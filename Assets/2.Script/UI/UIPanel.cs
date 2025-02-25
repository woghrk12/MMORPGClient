using UnityEngine;

public class UIPanel : UIBase
{
    #region Variables

    private Canvas canvas = null;

    #endregion Variables

    #region Properties

    public int SortingOrder { set { canvas.sortingOrder = value; } get => canvas.sortingOrder; }

    #endregion Properties

    #region Unity Events

    protected virtual void Awake()
    {
        canvas = GetComponent<Canvas>();

        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;
        canvas.sortingOrder = 0;
    }

    #endregion Unity Events
}
