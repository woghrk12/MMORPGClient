using Google.Protobuf.Protocol;
using UnityEngine;

public class Character : Creature
{
    #region Properties

    public override EGameObjectType GameObjectType => EGameObjectType.Character;

    #endregion Properties

    #region Methods

    public virtual void OnRevive(Vector3Int revivePos)
    {
        MoveDirection = EMoveDirection.None;
        IsCollidable = true;
        CurHp = MaxHp;

        Managers.Map.MoveObject(this, revivePos);

        transform.position = new Vector3(Position.x, Position.y) + new Vector3(0.5f, 0.5f);

        CurState = ECreatureState.Idle;
    }

    #endregion Methods
}
