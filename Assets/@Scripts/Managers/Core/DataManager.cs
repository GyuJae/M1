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
    public Dictionary<int, CreatureData> CreatureDic { get; private set; } = new();
    public Dictionary<int, EnvData> EnvDic { get; private set; } = new();

    public void Init()
    {
        CreatureDic = LoadJson<CreatureDataLoader, int, CreatureData>("CreatureData").MakeDict();
        EnvDic = LoadJson<EnvDataLoader, int, EnvData>("EnvData").MakeDict();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        var textAsset = Managers.Resource.Load<TextAsset>(path);
        return JsonConvert.DeserializeObject<Loader>(textAsset.text);
    }
}
