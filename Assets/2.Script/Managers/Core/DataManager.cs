using Google.Protobuf.Protocol;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
    #region Properties

    public Dictionary<int, Data.ObjectStat> ObjectStatDictionary { private set; get; } = new();
    public Dictionary<int, Data.AttackStat> AttackStatDictionary { private set; get; } = new();
    public Dictionary<int, Data.ProjectileStat> ProjectileStatDictionary { private set; get; } = new();

    #endregion Properties

    #region Methods

    public void SetData(EStatType statType, string data)
    {
        switch (statType)
        {
            case EStatType.ObjectData:
                List<Data.ObjectStat> objectStatList = JsonConvert.DeserializeObject<List<Data.ObjectStat>>(data);
                
                foreach (Data.ObjectStat stat in objectStatList)
                {
                    ObjectStatDictionary.Add(stat.ID, stat);
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
        }

        Debug.LogError($"Not supported data type. Input type : {statType}");
    }

    #endregion Methods
}
