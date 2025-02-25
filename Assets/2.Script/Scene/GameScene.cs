public class GameScene : BaseScene
{
    #region Variables

    private UIGameScene sceneUI = null;

    #endregion Variables
    
    #region Methods

    protected override void Init()
    {
        base.Init();

        SceneType = EScene.GAME;

        // Set scene ui
        sceneUI = Managers.UI.OpenSceneUI<UIGameScene>();

        Managers.Map.LoadMap(1);
    }

    public override void Clear()
    {

    }

    #endregion Methods
}
