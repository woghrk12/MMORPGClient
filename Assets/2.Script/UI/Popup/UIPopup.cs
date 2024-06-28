using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopup : UIBase
{
    #region Methods

    public virtual void Init()
    { 
        Managers.UI.SetCanvas(gameObject);
    }

    public virtual void CloseUI()
    {
        Managers.UI.ClosePopupUI(this);
    }

    #endregion Methods
}
