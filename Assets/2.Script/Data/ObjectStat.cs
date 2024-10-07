using System.Collections.Generic;

namespace Data
{
    public class ObjectStat
    {
        public int ID;
        public string Name;
        public Dictionary<int, int> MaxHpDictionary = new();
        public Dictionary<int, int> AttackPowerDictionary = new();
    }
}