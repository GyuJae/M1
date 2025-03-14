﻿using System;
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
        public string IconImage; // NEW
        public string SkeletonDataID;
        public string AnimatorName;
        public List<int> SkillIdList = new();
        public int DropItemId;
    }

    #endregion

    #region MonsterData

    [Serializable]
    public class MonsterData : CreatureData
    {
    }

    [Serializable]
    public class MonsterDataLoader : ILoader<int, MonsterData>
    {
        public List<MonsterData> monsters = new();

        public Dictionary<int, MonsterData> MakeDict()
        {
            var dict = new Dictionary<int, MonsterData>();
            foreach (var monster in monsters)
                dict.Add(monster.DataId, monster);
            return dict;
        }
    }

    #endregion

    #region HeroData

    [Serializable]
    public class HeroData : CreatureData
    {
    }

    [Serializable]
    public class HeroDataLoader : ILoader<int, HeroData>
    {
        public List<HeroData> heroes = new();

        public Dictionary<int, HeroData> MakeDict()
        {
            var dict = new Dictionary<int, HeroData>();
            foreach (var hero in heroes)
                dict.Add(hero.DataId, hero);
            return dict;
        }
    }

    #endregion

    #region SkillData

    [Serializable]
    public class SkillData
    {
        public int DataId;
        public string Name;
        public string ClassName;
        public string ComponentName;
        public string Description;
        public int ProjectileId;
        public string PrefabLabel;
        public string IconLabel;
        public string AnimName;
        public float CoolTime;
        public float DamageMultiplier;
        public float Duration;
        public float NumProjectiles;
        public string CastingSound;
        public float AngleBetweenProj;
        public float SkillRange;
        public float RotateSpeed;
        public float ScaleMultiplier;
        public float AngleRange;
    }

    [Serializable]
    public class SkillDataLoader : ILoader<int, SkillData>
    {
        public List<SkillData> skills = new();

        public Dictionary<int, SkillData> MakeDict()
        {
            var dict = new Dictionary<int, SkillData>();
            foreach (var skill in skills)
                dict.Add(skill.DataId, skill);
            return dict;
        }
    }

    #endregion

    #region ProjectileData

    [Serializable]
    public class ProjectileData
    {
        public int DataId;
        public string Name;
        public string ComponentName;
        public string ProjectileSpriteName;
        public string PrefabLabel;
        public float Duration;
        public float NumBounce;
        public float NumPenerations;
        public float HitSound;
        public float ProjRange;
        public float ProjSpeed;
    }

    [Serializable]
    public class ProjectileDataLoader : ILoader<int, ProjectileData>
    {
        public List<ProjectileData> projectiles = new();

        public Dictionary<int, ProjectileData> MakeDict()
        {
            var dict = new Dictionary<int, ProjectileData>();
            foreach (var projectile in projectiles)
                dict.Add(projectile.DataId, projectile);
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
