using UnityEngine;
using static Define;

public class GameScene : BaseScene
{
    public override bool Init()
    {
        if (base.Init() == false) return false;
        SceneType = EScene.GameScene;

        var map = Managers.Resource.Instantiate("BaseMap");
        map.transform.position = Vector3.zero;
        map.name = "@BaseMap";

        return true;
    }

    public override void Clear()
    {

    }
}
