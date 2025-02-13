public abstract class UIPopup : UIBase
{
    #region Methods

    public virtual void CloseUI()
    {
        Managers.UI.ClosePopupUI(this);
    }

    #endregion Methods
}
