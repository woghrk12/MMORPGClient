using System.Collections.Generic;
using UnityEngine;

public class UIInventory : UIPopup
{
    #region Variables

    private int tap = 1;
    private List<UIInventoryItem> invenItemList = new();

    [SerializeField] private Transform itemGridTransform = null;

    #endregion Variables

    #region Unity Events

    protected override void Awake()
    {
        base.Awake();

        foreach (Transform invenItem in itemGridTransform)
        {
            invenItemList.Add(invenItem.GetComponent<UIInventoryItem>());
        }
    }

    private void OnEnable()
    {
        RefreshUI();
    }

    #endregion Unity Events

    #region Methods

    public void RefreshUI()
    {
        List<Item> itemList = Managers.Inventory.GetAllItems();

        foreach (Item item in itemList)
        {
            if ((item.Slot >> 24) != tap) continue;

            invenItemList[item.Slot & 0xFFFFFF].SetSlot(item.TemplateID, item.Count);
        }
    }

    #endregion Methods
}
