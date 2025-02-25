public abstract class UIPopup : UIPanel
{
    #region Methods

    public virtual void CloseUI()
    {
        Managers.UI.ClosePopupUI(this);
    }

    #endregion Methods
}
