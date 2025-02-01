using System.Collections.Generic;
using Data;
using Newtonsoft.Json;
using UnityEngine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    public Dictionary<int, MonsterData> MonsterDic { get; private set; } = new();
    public Dictionary<int, HeroData> HeroDic { get; private set; } = new();
    public Dictionary<int, SkillData> SkillDic { get; private set; } = new();
    public Dictionary<int, ProjectileData> ProjectileDic { get; private set; } = new();
    public Dictionary<int, EnvData> EnvDic { get; private set; } = new();

    public void Init()
    {
        MonsterDic = LoadJson<MonsterDataLoader, int, MonsterData>("MonsterData").MakeDict();
        HeroDic = LoadJson<HeroDataLoader, int, HeroData>("HeroData").MakeDict();
        SkillDic = LoadJson<SkillDataLoader, int, SkillData>("SkillData").MakeDict();
        ProjectileDic = LoadJson<ProjectileDataLoader, int, ProjectileData>("ProjectileData").MakeDict();
        EnvDic = LoadJson<EnvDataLoader, int, EnvData>("EnvData").MakeDict();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        var textAsset = Managers.Resource.Load<TextAsset>(path);
        return JsonConvert.DeserializeObject<Loader>(textAsset.text);
    }
}
