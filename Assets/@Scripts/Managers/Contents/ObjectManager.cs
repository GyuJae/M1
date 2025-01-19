using System;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class ObjectManager
{
    public HashSet<Hero> Heroes { get; } = new();
    public HashSet<Monster> Monsters { get; } = new();
    public HashSet<Env> Envs { get; } = new();
    public HeroCamp Camp { get; private set; }


    public T Spawn<T>(Vector3 position, int templateID) where T : BaseObject
    {
        var prefabName = typeof(T).Name;

        var go = Managers.Resource.Instantiate(prefabName);
        go.name = prefabName;
        go.transform.position = position;

        var obj = go.GetComponent<BaseObject>();

        switch (obj.ObjectType)
        {
            case EObjectType.Creature:
            {
                // Data Check
                if (templateID != 0 && Managers.Data.CreatureDic.TryGetValue(templateID, out var creatureData) == false)
                {
                    Debug.LogError($"ObjectManager Spawn Creature Failed! TryGetValue TemplateID : {templateID}");
                    return null;
                }

                var creature = go.GetComponent<Creature>();
                switch (creature.CreatureType)
                {
                    case ECreatureType.Hero:
                        obj.transform.parent = HeroRoot;
                        var hero = creature as Hero;
                        Heroes.Add(hero);
                        break;
                    case ECreatureType.Monster:
                        obj.transform.parent = MonsterRoot;
                        var monster = creature as Monster;
                        Monsters.Add(monster);
                        break;
                    case ECreatureType.None:
                    case ECreatureType.Npc:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                creature.SetInfo(templateID);
                break;
            }
            case EObjectType.Projectile:
                break;
            case EObjectType.Env:
                // Data Check
                if (templateID != 0 && Managers.Data.EnvDic.TryGetValue(templateID, out var envData) == false)
                {
                    Debug.LogError($"ObjectManager Spawn Env Failed! TryGetValue TemplateID : {templateID}");
                    return null;
                }

                obj.transform.parent = EnvRoot;

                var env = go.GetComponent<Env>();
                Envs.Add(env);

                env.SetInfo(templateID);
                break;
            case EObjectType.None:
                break;
            case EObjectType.HeroCamp:
                Camp = go.GetComponent<HeroCamp
                >();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return obj as T;
    }

    public void Despawn<T>(T obj) where T : BaseObject
    {
        var objectType = obj.ObjectType;

        switch (obj.ObjectType)
        {
            case EObjectType.Creature:
            {
                var creature = obj.GetComponent<Creature>();
                switch (creature.CreatureType)
                {
                    case ECreatureType.Hero:
                        var hero = creature as Hero;
                        Heroes.Remove(hero);
                        break;
                    case ECreatureType.Monster:
                        var monster = creature as Monster;
                        Monsters.Remove(monster);
                        break;
                }
                break;
            }
            case EObjectType.Projectile:
            case EObjectType.Env:
                var env = obj as Env;
                Envs.Remove(env);
                break;
        }
        Managers.Resource.Destroy(obj.gameObject);
    }

    #region Roots

    public Transform GetRootTransform(string name)
    {
        var root = GameObject.Find(name);
        if (root == null)
            root = new GameObject { name = name };

        return root.transform;
    }

    public Transform HeroRoot
    {
        get { return GetRootTransform("@Heroes"); }
    }
    public Transform MonsterRoot
    {
        get { return GetRootTransform("@Monsters"); }
    }
    public Transform EnvRoot
    {
        get { return GetRootTransform("@Envs"); }
    }

    #endregion
}
