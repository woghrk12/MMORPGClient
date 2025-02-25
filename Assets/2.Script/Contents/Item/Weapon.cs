using Data;
using Google.Protobuf.Protocol;

public class Weapon : Item
{
    #region Properties

    public override EItemType Type => EItemType.ItemTypeWeapon;

    public override bool IsStackable => false;

    public EWeaponType WeaponType { private set; get; } = EWeaponType.WeaponTypeNone;

    public int Value { private set; get; } = 0;

    #endregion Properties

    #region Constructor

    public Weapon(int id, int templateID) : base(id, templateID)
    {
        if (Managers.Data.ItemStatDictionary.TryGetValue(templateID, out ItemStat stat) == false) return;

        WeaponStat weaponStat = stat as WeaponStat;

        Count = 1;
        WeaponType = weaponStat.Type;
        Value = weaponStat.Value;
    }

    #endregion Constructor
}
