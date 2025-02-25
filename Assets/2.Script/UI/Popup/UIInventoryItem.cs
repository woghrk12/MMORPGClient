using Data;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryItem : UIBase
{
    #region Variables

    [SerializeField] private Image iconImage = null;
    [SerializeField] private Text countText = null;

    #endregion Variables

    #region Methods

    public void SetSlot(int templateID, int count)
    {
        if (Managers.Data.ItemStatDictionary.TryGetValue(templateID, out ItemStat stat) == false) return;

        Sprite iconSprite = Managers.Resource.Load<Sprite>(stat.IconPath);
        iconImage.sprite = iconSprite;

        countText.gameObject.SetActive(count > 1);
        countText.text = count.ToString();
    }

    #endregion Methods
}
