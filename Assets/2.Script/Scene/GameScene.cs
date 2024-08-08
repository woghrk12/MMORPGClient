using UnityEngine;

public class GameScene : BaseScene
{
    #region Methods

    protected override void Init()
    {
        base.Init();

        SceneType = EScene.GAME;

        Managers.Map.LoadMap(1);
    }

    public override void Clear()
    {

    }

    #endregion Methods
}
