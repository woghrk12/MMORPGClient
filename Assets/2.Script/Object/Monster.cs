using Google.Protobuf.Protocol;

public class Monster : Creature
{
    #region Properties

    public override EGameObjectType GameObjectType => EGameObjectType.Monster;

    #endregion Properties
}
