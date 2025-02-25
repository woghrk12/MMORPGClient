using Data;
using Google.Protobuf.Protocol;

public abstract class Item
{
    #region Variables

    private ItemInfo info = new();

    #endregion Variables

    #region Properties

    public int ID => info.Id;

    public int TemplateID => info.TemplateID;

    public int Count
    {
        set { info.Count = value; }
        get => info.Count;
    }

    // [UNUSED(1)][TAP(7)][SLOT(24)] 
    public int Slot
    {
        set { info.Slot = value; }
        get => info.Slot;
    }

    public abstract EItemType ItemType { get; }

    public abstract bool IsStackable { get; }

    #endregion Properties

    #region Constructor

    public Item(int id, int templateID)
    {
        info.Id = id;
        info.TemplateID = templateID;
    }

    #endregion Constructor

    #region Methods

    public static Item MakeItem(ItemInfo target)
    {
        if (Managers.Data.ItemStatDictionary.TryGetValue(target.TemplateID, out ItemStat stat) == false) return null;

        switch (stat.ItemType)
        {
            case EItemType.ItemTypeEquipment:
                EquipmentStat equipmentStat = stat as EquipmentStat;

                if (ReferenceEquals(equipmentStat, null) == true) return null;

                switch (equipmentStat.EquipmentType)
                {
                    case EEquipmentType.EquipmentTypeWeapon:
                        return new Weapon(target.Id, target.TemplateID);

                    case EEquipmentType.EquipmentTypeArmor:
                        return new Armor(target.Id, target.TemplateID);

                    default:
                        return null;
                }

            case EItemType.ItemTypeConsumable:
                return new Consumable(target.Id, target.TemplateID, target.Count);

            case EItemType.ItemTypeLoot:
                return new Loot(target.Id, target.TemplateID, target.Count);
        }

        return null;
    }

    #endregion Methods
}
