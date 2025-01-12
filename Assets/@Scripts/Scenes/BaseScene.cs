using UnityEngine;
using UnityEngine.EventSystems;
using static Define;

public abstract class BaseScene : InitBase
{
    protected EScene SceneType { get; set; } = Define.EScene.Unknown;
    
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        var obj = GameObject.FindObjectOfType(typeof(EventSystem));
        
        if (obj != null) return true;
        
        var go = new GameObject() { name = "@EventSystem" };
        go.AddComponent<EventSystem>();
        go.AddComponent<StandaloneInputModule>();

        return true;
    }

    public abstract void Clear();
}
