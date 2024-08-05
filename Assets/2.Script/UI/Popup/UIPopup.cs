public class UIPopup : UIBase
{
    #region Methods

    protected virtual void Init()
    { 
        Managers.UI.SetCanvas(gameObject);
    }

    public virtual void CloseUI()
    {
        Managers.UI.ClosePopupUI(this);
    }

    #endregion Methods
}
