using Google.Protobuf.Protocol;
using UnityEngine;

public class Character : Creature
{
    #region Properties

    public override EGameObjectType GameObjectType => EGameObjectType.Character;

    #endregion Properties

    #region Methods

    public virtual void OnRevive(ObjectInfo objectInfo)
    {
        Managers.Map.MoveObject(this, new Vector2Int(objectInfo.PosX, objectInfo.PosY));
        transform.position = new Vector3(Position.x, Position.y) + new Vector3(0.5f, 0.5f);

        IsCollidable = objectInfo.IsCollidable;
        CurState = objectInfo.CreatureInfo.CurState;
        MoveDirection = objectInfo.CreatureInfo.MoveDirection;
        FacingDirection = objectInfo.CreatureInfo.FacingDirection;
        MoveSpeed = objectInfo.CreatureInfo.MoveSpeed;

        Level = objectInfo.CreatureInfo.Stat.Level;
        MaxHp = objectInfo.CreatureInfo.Stat.MaxHP;
        CurHp = objectInfo.CreatureInfo.Stat.CurHP;
        AttackPower = objectInfo.CreatureInfo.Stat.AttackPower;

        CachedSpriteRenderer.enabled = true;
    }

    #endregion Methods
}
