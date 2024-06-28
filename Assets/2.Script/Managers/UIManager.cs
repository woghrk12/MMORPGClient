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
        Canvas canvas = go.GetOrAddComponent<Canvas>();

        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;
        canvas.sortingOrder = isSort ? sortingOrder++ : 0;
    }

    public void Clear()
    {
        CloseAllPopupUI();
        CloseSceneUI();
    }

    #region Scene UI

    public T ShowSceneUI<T>(string name = null) where T : UIScene
    {
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }

        GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");
        T sceneUI = go.GetOrAddComponent<T>();

        this.sceneUI = sceneUI;

        go.transform.SetParent(Root.transform);

        return sceneUI;
    }

    public void CloseSceneUI()
    {
        if (ReferenceEquals(sceneUI, null)) return;

        Managers.Resource.Destory(sceneUI.gameObject);
        sceneUI = null;
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
        T popupUI = go.GetOrAddComponent<T>();

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

    #region Sub Item

    public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UIBase
    {
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }

        GameObject go = Managers.Resource.Instantiate($"UI/SubItem/{name}");

        if (!ReferenceEquals(parent, null))
        {
            go.transform.SetParent(parent);
        }

        return go.GetOrAddComponent<T>();
    }

    #endregion Sub Item

    #endregion Methods
}
