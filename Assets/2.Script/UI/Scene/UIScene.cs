public class UIScene : UIBase
{
    #region Methods

    protected virtual void Init()
    { 
        Managers.UI.SetCanvas(gameObject, false);
    }

    #endregion Methods
}
