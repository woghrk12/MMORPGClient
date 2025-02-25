using Data;
using Google.Protobuf.Protocol;

public class Armor : Item
{
    #region Properties

    public override EItemType Type => EItemType.ItemTypeArmor;

    public override bool IsStackable => false;

    public EArmorType ArmorType { private set; get; } = EArmorType.ArmorTypeNone;

    public int Value { private set; get; } = 0;

    #endregion Properties

    #region Constructor

    public Armor(int id, int templateID) : base(id, templateID)
    {
        if (Managers.Data.ItemStatDictionary.TryGetValue(templateID, out ItemStat stat) == false) return;
        
        ArmorStat armorStat = stat as ArmorStat;

        Count = 1;
        ArmorType = armorStat.Type;
        Value = armorStat.Value;
    }

    #endregion Constructor
}
