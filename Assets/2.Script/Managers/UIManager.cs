using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    #region Variables

    private int sortingOrder = 10;

    private UIScene sceneUI = null;
    private Stack<UIPopup> popupUIStack = new();

    #endregion Variables

    #region Properties

    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UIRoot");

            if (ReferenceEquals(root, null))
            {
                root = new GameObject { name = "@UIRoot" };
            }

            return root;
        }
    }

    #endregion Properties

    #region Methods

    public void SetCanvas(GameObject go, bool isSort = true)
    {
        Canvas canvas = Utility.GetOrAddComponent<Canvas>(go);

        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;
        canvas.sortingOrder = isSort ? sortingOrder++ : 0;
    }

    #region Scene UI

    public T ShowSceneUI<T>(string name = null) where T : UIScene
    {
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }

        GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");
        T sceneUI = Utility.GetOrAddComponent<T>(go);

        this.sceneUI = sceneUI;

        go.transform.SetParent(Root.transform);

        return sceneUI;
    }

    #endregion Scene UI

    #region Popup UI

    public T ShowPopupUI<T>(string name = null) where T : UIPopup
    {
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }

        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}");
        T popupUI = Utility.GetOrAddComponent<T>(go);

        popupUIStack.Push(popupUI);

        go.transform.SetParent(Root.transform);

        return popupUI;
    }

    public void ClosePopupUI()
    {
        if (popupUIStack.Count == 0) return;

        UIPopup popupUI = popupUIStack.Pop();

        Managers.Resource.Destory(popupUI.gameObject);

        sortingOrder--;
    }

    public void ClosePopupUI(UIPopup popup)
    {
        if (popupUIStack.Count == 0) return;

        if (ReferenceEquals(popupUIStack.Peek(), popup))
        {
            Debug.LogWarning($"Failed to close popup UI {popup.name}.");
        }

        ClosePopupUI();
    }

    public void CloseAllPopupUI()
    {
        while (popupUIStack.Count > 0)
        {
            ClosePopupUI();
        }
    }

    #endregion Popup UI

    #endregion Methods
}
