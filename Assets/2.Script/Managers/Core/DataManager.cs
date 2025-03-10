using Google.Protobuf.Protocol;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
    #region Properties

    public Dictionary<int, Data.CharacterStat> CharacterStatDictionary { private set; get; } = new();
    public Dictionary<int, Data.MonsterStat> MonsterStatDictionary { private set; get; } = new();
    public Dictionary<int, Data.AttackStat> AttackStatDictionary { private set; get; } = new();
    public Dictionary<int, Data.ProjectileStat> ProjectileStatDictionary { private set; get; } = new();
    public Dictionary<int, Data.ItemStat> ItemStatDictionary { private set; get; } = new();

    #endregion Properties

    #region Methods

    public void SetData(EStatType statType, string data)
    {
        switch (statType)
        {
            case EStatType.CharacterData:
                List<Data.CharacterStat> characterStatList = JsonConvert.DeserializeObject<List<Data.CharacterStat>>(data);
                
                foreach (Data.CharacterStat stat in characterStatList)
                {
                    CharacterStatDictionary.Add(stat.ID, stat);
                }
                
                return;

            case EStatType.MonsterData:
                List<Data.MonsterStat> monsterStatList = JsonConvert.DeserializeObject<List<Data.MonsterStat>>(data);

                foreach (Data.MonsterStat stat in monsterStatList)
                {
                    MonsterStatDictionary.Add(stat.ID, stat);
                }

                return;

            case EStatType.AttackData:
                List<Data.AttackStat> attackStatList = JsonConvert.DeserializeObject<List<Data.AttackStat>>(data);

                foreach (Data.AttackStat stat in attackStatList)
                {
                    AttackStatDictionary.Add(stat.ID, stat);
                }

                return;

            case EStatType.ProjectileData:
                List<Data.ProjectileStat> projectileStatList = JsonConvert.DeserializeObject<List<Data.ProjectileStat>>(data);

                foreach (Data.ProjectileStat stat in projectileStatList)
                {
                    ProjectileStatDictionary.Add(stat.ID, stat);
                }

                return;

            case EStatType.ItemData:
                List<Data.ItemStat> itemStatList = JsonConvert.DeserializeObject<List<Data.ItemStat>>(data);

                foreach (Data.ItemStat stat in itemStatList)
                {
                    ItemStatDictionary.Add(stat.ID, stat);
                }

                return;
        }

        Debug.LogError($"Not supported data type. Input type : {statType}");
    }

    #endregion Methods
}
