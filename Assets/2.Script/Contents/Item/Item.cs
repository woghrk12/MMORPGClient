using Google.Protobuf.Protocol;

public abstract class Item
{
    #region Variables

    private ItemInfo info = new();

    #endregion Variables

    #region Properties

    public int ID { private set; get; } = -1;

    public int TemplateID
    {
        private set { info.TemplateID = value; }
        get => info.TemplateID;
    }

    public int Count
    {
        set { info.Count = value; }
        get => info.Count;
    }

    public int Slot
    {
        set { info.Slot = value; }
        get => info.Slot;
    }

    public abstract EItemType Type { get; }

    public abstract bool IsStackable { get; }

    #endregion Properties

    #region Constructor

    public Item(int id, int templateID)
    {
        ID = id;
        TemplateID = templateID;
    }

    #endregion Constructor

    #region Methods

    public static Item MakeItem(ItemInfo target)
    {
        if (Managers.Data.ItemStatDictionary.TryGetValue(target.TemplateID, out Data.ItemStat stat) == false) return null;

        switch (stat.ItemType)
        {
            case EItemType.ItemTypeWeapon:
                return new Weapon(target.Id, target.TemplateID);

            case EItemType.ItemTypeArmor:
                return new Armor(target.Id, target.TemplateID);

            case EItemType.ItemTypeConsumable:
                return new Consumable(target.Id, target.TemplateID, target.Count);
        }

        return null;
    }

    #endregion Methods
}
