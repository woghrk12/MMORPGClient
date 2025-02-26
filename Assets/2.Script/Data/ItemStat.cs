using Google.Protobuf.Protocol;

namespace Data
{
    public class ItemStat
    {
        public int ID;
        public string Name;
        public EItemType ItemType;
        public string IconPath;
    }

    public class EquipmentStat : ItemStat
    {
        public EEquipmentType EquipmentType;
    }

    // [UNUSED(1)][ITEM_TYPE(3)][EQUIPMENT_TYPE(4)][WEAPON_TYPE(4)][ID(20)]
    public class WeaponStat : EquipmentStat
    {
        public EWeaponType WeaponType;
        public int Value;
    }

    // [UNUSED(1)][ITEM_TYPE(3)][EQUIPMENT_TYPE(4)][ARMOR_TYPE(4)][ID(20)]
    public class ArmorStat : EquipmentStat
    {
        public EArmorType ArmorType;
        public int Value;
    }

    // [UNUSED(1)][ITEM_TYPE(3)][CONSUMABLE_TYPE(8)][ID(20)]
    public class ConsumableStat : ItemStat
    {
        public EConsumableType ConsumableType;
        public int Value;
        public int MaxCount;
    }

    // [UNUSED(1)][ITEM_TYPE(3)][ID(28)]
    public class LootStat : ItemStat
    {
        public int MaxCount;
    }
}
