using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

public class ResourceManager
{
	readonly Dictionary<string, Object> resources = new();
	readonly Dictionary<string, AsyncOperationHandle> handles = new();

	#region Load Resource
	public T Load<T>(string key) where T : Object
	{
		if (resources.TryGetValue(key, out Object resource))
			return resource as T;

		return null;
	}

	public GameObject Instantiate(string key, Transform parent = null, bool pooling = false)
	{
		GameObject prefab = Load<GameObject>(key);
		if (prefab == null)
		{
			Debug.LogError($"Failed to load prefab : {key}");
			return null;
		}

		// TODO
		// if (pooling)
		// 	return Managers.Pool.Pop(prefab);

		GameObject go = Object.Instantiate(prefab, parent);
		go.name = prefab.name;

		return go;
	}

	public void Destroy(GameObject go)
	{
		if (go == null)
			return;

		// TODO
		// if (Managers.Pool.Push(go))
		// 	return;

		Object.Destroy(go);
	}
	#endregion

	#region Addressable
	private void LoadAsync<T>(string key, Action<T> callback = null) where T : UnityEngine.Object
	{
		// Cache
		if (resources.TryGetValue(key, out Object resource))
		{
			callback?.Invoke(resource as T);
			return;
		}

		string loadKey = key;
		if (key.Contains(".sprite"))
			loadKey = $"{key}[{key.Replace(".sprite", "")}]";

		var asyncOperation = Addressables.LoadAssetAsync<T>(loadKey);
		asyncOperation.Completed += (op) =>
		{
			resources.Add(key, op.Result);
			handles.Add(key, asyncOperation);
			callback?.Invoke(op.Result);
		};
	}

	public void LoadAllAsync<T>(string label, Action<string, int, int> callback) where T : Object
	{
		var opHandle = Addressables.LoadResourceLocationsAsync(label, typeof(T));
		opHandle.Completed += (op) =>
		{
			int loadCount = 0;
			int totalCount = op.Result.Count;

			foreach (var result in op.Result)
			{
				if (result.PrimaryKey.Contains(".sprite"))
				{
					LoadAsync<Sprite>(result.PrimaryKey, (obj) =>
					{
						loadCount++;
						callback?.Invoke(result.PrimaryKey, loadCount, totalCount);
					});
				}
				else
				{
					LoadAsync<T>(result.PrimaryKey, (obj) =>
					{
						loadCount++;
						callback?.Invoke(result.PrimaryKey, loadCount, totalCount);
					});
				}
			}
		};
	}

	public void Clear()
	{
		resources.Clear();

		foreach (var handle in handles)
			Addressables.Release(handle);

		handles.Clear();
	}
	#endregion
}
