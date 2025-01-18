using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public static class Extension
{
    public static T GetOrAddComponent<T>(this GameObject go) where T : Component
    {
        return Util.GetOrAddComponent<T>(go);
    }

    public static void BindEvent(this GameObject go, Action<PointerEventData> action = null,
        Define.EUIEvent type = Define.EUIEvent.Click)
    {
        UI_Base.BindEvent(go, action, type);
    }

    public static void TranslateEx(this Transform transform, Vector3 dir)
    {
        var bo = transform.gameObject.GetComponent<BaseObject>();
        if (bo != null)
            bo.TranslateEx(dir);
    }

    public static bool IsValid(this GameObject go)
    {
        return go != null && go.activeSelf;
    }

    public static void DestroyChilds(this GameObject go)
    {
        foreach (Transform child in go.transform)
            Managers.Resource.Destroy(child.gameObject);
    }

    public static void Shuffle<T>(this IList<T> list)
    {
        var n = list.Count;

        while (n > 1)
        {
            n--;
            var k = Random.Range(0, n + 1);
            (list[k], list[n]) = (list[n], list[k]); //swap
        }
    }
}
