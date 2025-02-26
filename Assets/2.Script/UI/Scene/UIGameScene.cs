public class UIGameScene : UIScene
{
    protected override void Awake()
    {
        base.Awake();

        Managers.UI.AddPopupUI<UIInventory>();
        Managers.UI.AddPopupUI<UIDead>();
    }
}
