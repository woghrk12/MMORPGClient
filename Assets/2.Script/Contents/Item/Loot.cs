using Data;
using Google.Protobuf.Protocol;

public class Loot : Item
{
    #region Properties

    public override EItemType ItemType => EItemType.ItemTypeLoot;

    public override bool IsStackable => MaxCount > 1;

    public int MaxCount { private set; get; } = 0;

    #endregion Properties

    #region Constructor

    public Loot(int id, int templateID, int slot, int count = 1) : base(id, templateID, slot)
    {
        if (Managers.Data.ItemStatDictionary.TryGetValue(templateID, out ItemStat stat) == false) return;

        LootStat lootStat = stat as LootStat;

        if (ReferenceEquals(lootStat, null) == true) return;
        if (lootStat.ItemType != EItemType.ItemTypeLoot) return;

        Count = count;
        MaxCount = lootStat.MaxCount;
    }

    #endregion Constructor
}
