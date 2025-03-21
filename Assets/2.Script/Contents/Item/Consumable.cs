using Data;
using Google.Protobuf.Protocol;

public class Consumable : Item
{
    #region Properties

    public override EItemType ItemType => EItemType.ItemTypeConsumable;
    public EConsumableType ConsumableType { private set; get; } = EConsumableType.ConsumableTypeNone;

    public override bool IsStackable => MaxCount > 1;

    public int Value { private set; get; } = 0;

    public int MaxCount { private set; get; } = 0;

    #endregion Properties

    #region Constructor

    public Consumable(int id, int templateID, int slot, int count = 1) : base(id, templateID, slot)
    {
        if (Managers.Data.ItemStatDictionary.TryGetValue(templateID, out ItemStat stat) == false) return;

        ConsumableStat consumableStat = stat as ConsumableStat;

        if (ReferenceEquals(consumableStat, null) == true) return;
        if (consumableStat.ItemType != EItemType.ItemTypeConsumable) return;

        Count = count;
        ConsumableType = consumableStat.ConsumableType;
        Value = consumableStat.Value;
        MaxCount = consumableStat.MaxCount;
    }

    #endregion Constructor
}
