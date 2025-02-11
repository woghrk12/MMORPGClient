using Google.Protobuf.Protocol;

namespace Data
{
    public class ItemStat
    {
        public int TemplateID;
        public string Name;
        public EItemType ItemType;
    }

    public class WeaponStat : ItemStat
    {
        public EWeaponType Type;
        public int Value;
    }

    public class ArmorStat : ItemStat
    {
        public EArmorType Type;
        public int Value;
    }

    public class ConsumableStat : ItemStat
    {
        public EConsumableType Type;
        public int Value;
        public int MaxCount;
    }
}
