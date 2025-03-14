using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

internal class Pool
{
    readonly GameObject prefab;
    readonly IObjectPool<GameObject> pool;

    Transform root;
    Transform Root
    {
        get
        {
            if (root == null)
            {
                GameObject go = new GameObject() { name = $"@{prefab.name}Pool" };
                root = go.transform;
            }

            return root;
        }
    }

    public Pool(GameObject prefab)
    {
        this.prefab = prefab;
        pool = new ObjectPool<GameObject>(OnCreate, OnGet, OnRelease, OnDestroy);
    }

    public void Push(GameObject go)
    {
        if (go.activeSelf)
            pool.Release(go);
    }

    public GameObject Pop()
    {
        return pool.Get();
    }

    #region Funcs
    GameObject OnCreate()
    {
        GameObject go = GameObject.Instantiate(prefab);
        go.transform.SetParent(Root);
        go.name = prefab.name;
        return go;
    }

    static void OnGet(GameObject go)
    {
        go.SetActive(true);
    }

    static void OnRelease(GameObject go)
    {
        go.SetActive(false);
    }

    static void OnDestroy(GameObject go)
    {
        GameObject.Destroy(go);
    }
    #endregion
}


public class PoolManager 
{
    readonly Dictionary<string, Pool> pools = new();

    public GameObject Pop(GameObject prefab)
    {
        if (pools.ContainsKey(prefab.name) == false)
            CreatePool(prefab);

        return pools[prefab.name].Pop();
    }

    public bool Push(GameObject go)
    {
        if (pools.TryGetValue(go.name, out var pool) == false)
            return false;

        pool.Push(go);
        return true;
    }

    public void Clear()
    {
        pools.Clear();
    }
    void CreatePool(GameObject original)
    {
        Pool pool = new Pool(original);
        pools.Add(original.name, pool);
    }
}
