using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    #region Variables

    private static readonly int baseSortingOrder = 10;

    private UIScene sceneUI = null;
    private Dictionary<int, UIPopup> popupUIDIctionary = new();

    private int sortingOrder = 0;
    private List<UIPopup> popupUIList = new();

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

    public void Clear()
    {
        CloseAllPopupUI();
        CloseSceneUI();
    }

    #region Scene UI

    public T OpenSceneUI<T>() where T : UIScene
    {
        GameObject go = Managers.Resource.Instantiate($"UI/Scene/{typeof(T).Name}");
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

    public T AddPopupUI<T>() where T : UIPopup
    {
        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{typeof(T).Name}");
        T popupUI = go.GetOrAddComponent<T>();

        popupUIDIctionary.Add(typeof(T).GetHashCode(), popupUI);

        go.transform.SetParent(Root.transform);
        go.gameObject.SetActive(false);

        return popupUI;
    }

    public T OpenPopupUI<T>() where T : UIPopup
    {
        int typeID = typeof(T).GetHashCode();

        if (popupUIDIctionary.TryGetValue(typeID, out UIPopup popupUI) == false) return null;
        
        popupUIDIctionary.Remove(typeID);
        popupUIList.Add(popupUI);

        popupUI.gameObject.SetActive(true);

        popupUI.SortingOrder = sortingOrder;
        sortingOrder++;

        return popupUI as T;
    }

    public void ClosePopupUI()
    {
        if (popupUIList.Count == 0) return;

        UIPopup popupUI = popupUIList[sortingOrder - 1];

        popupUIList.RemoveAt(sortingOrder - 1);
        popupUIDIctionary.Add(popupUI.GetType().GetHashCode(), popupUI);

        popupUI.gameObject.SetActive(false);

        sortingOrder--;
    }

    public void ClosePopupUI(UIPopup target)
    {
        if (popupUIList.Count == 0) return;

        int index = popupUIList.IndexOf(target);
        if (index < 0) return;

        UIPopup popupUI = popupUIList[index];

        popupUIList.RemoveAt(index);
        popupUIDIctionary.Add(popupUI.GetType().GetHashCode(), popupUI);

        popupUI.gameObject.SetActive(false);

        sortingOrder--;

        for (int i = 0; i < sortingOrder; i++)
        {
            popupUIList[i].SortingOrder = i + baseSortingOrder;
        }
    }

    public void CloseAllPopupUI()
    {
        while (popupUIList.Count > 0)
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
