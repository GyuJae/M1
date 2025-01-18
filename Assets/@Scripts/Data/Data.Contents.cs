using System;
using System.Collections.Generic;

namespace Data
{
    #region CreatureData

    [Serializable]
    public class CreatureData
    {
        public int DataId;
        public string DescriptionTextID;
        public string PrefabLabel;
        public float ColliderOffsetX;
        public float ColliderOffstY;
        public float ColliderRadius;
        public float Mass;
        public float MaxHp;
        public float MaxHpBonus;
        public float Atk;
        public float AtkRange;
        public float AtkBonus;
        public float Def;
        public float MoveSpeed;
        public float TotalExp;
        public float HpRate;
        public float AtkRate;
        public float DefRate;
        public float MoveSpeedRate;
        public string SkeletonDataID;
        public string AnimatorName;
        public List<int> SkillIdList = new();
        public int DropItemId;
    }

    [Serializable]
    public class CreatureDataLoader : ILoader<int, CreatureData>
    {
        public List<CreatureData> creatures = new();

        public Dictionary<int, CreatureData> MakeDict()
        {
            var dict = new Dictionary<int, CreatureData>();
            foreach (var creature in creatures)
                dict.Add(creature.DataId, creature);
            return dict;
        }
    }

    #endregion

    #region Env

    [Serializable]
    public class EnvData
    {
        public int DataId;
        public string DescriptionTextID;
        public string PrefabLabel;
        public float MaxHp;
        public int ResourceAmount;
        public float RegenTime;
        public List<string> SkeletonDataIDs = new();
        public int DropItemId;
    }

    [Serializable]
    public class EnvDataLoader : ILoader<int, EnvData>
    {
        public List<EnvData> envs = new();

        public Dictionary<int, EnvData> MakeDict()
        {
            var dict = new Dictionary<int, EnvData>();
            foreach (var env in envs)
                dict.Add(env.DataId, env);
            return dict;
        }
    }

    #endregion
}
