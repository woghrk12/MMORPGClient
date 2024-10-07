using Google.Protobuf.Protocol;
using System.Collections.Generic;

namespace Data
{
    public class AttackStat
    {
        public int ID;
        public string Name;
        public string AnimationKey;
        public float Cooldown;
        public Dictionary<int, float> CoeffDictionary;
        public EAttackType AttackType;
        public int Range;
        public int ProjectileID;
    }
}