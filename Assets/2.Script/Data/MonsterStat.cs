using System.Collections.Generic;

namespace Data
{
    public class MonsterStat
    {
        public int ID;
        public string Name;
        public Dictionary<int, int> MaxHpDictionary = new();
        public Dictionary<int, int> AttackPowerDictionary = new();
        public int PatrolRange;
        public int DetectionRange;
        public int ChaseRange;
    }
}