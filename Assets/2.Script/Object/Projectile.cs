using Google.Protobuf.Protocol;
using UnityEngine;

public class Projectile : MMORPG.Object
{
    #region Variables

    private EMoveDirection moveDirection = EMoveDirection.None;

    #endregion Variables

    #region Properties

    public sealed override EGameObjectType GameObjectType => EGameObjectType.Projectile;

    public Creature Owner { set; get; } = null;

    public Data.AttackStat AttackStat { set; get; } = null;

    public EMoveDirection MoveDirection
    {
        set
        {
            if (moveDirection == value) return;

            moveDirection = value;

            switch (moveDirection)
            {
                case EMoveDirection.Up:
                    transform.eulerAngles = new Vector3(0f, 90f, 0f);
                    break;

                case EMoveDirection.Down:
                    transform.eulerAngles = new Vector3(0f, -90f, 0f);
                    break;

                case EMoveDirection.Left:
                    transform.eulerAngles = new Vector3(0f, 180f, 0f);
                    break;

                case EMoveDirection.Right:
                    transform.eulerAngles = new Vector3(0f, 0f, 0f);
                    break;
            }
        }
        get => moveDirection;
    }

    public int MoveSpeed { set; get; } = 5;

    #endregion Properties

    #region Methods

    public override void Init(ObjectInfo info)
    {
        ID = info.ObjectID;
        Name = info.Name;
        Position = new Vector3Int(info.PosX, info.PosY);
        IsCollidable = info.IsCollidable;

        MoveDirection = info.CreatureInfo.MoveDirection;
        MoveSpeed = info.CreatureInfo.MoveSpeed;
    }

    #endregion Methods
}
