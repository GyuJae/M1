using UnityEngine;
using static Define;

public class TitleScene : BaseScene
{
    public override bool Init()
    {
        if (base.Init() == false) return false;
        SceneType = EScene.TitleScene;
        
        
        return true;
    }

    public override void Clear()
    {
       
    }
}
