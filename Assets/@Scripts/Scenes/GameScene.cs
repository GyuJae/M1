using Unity.VisualScripting;
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

        var hero = Managers.Object.Spawn<Hero>(new Vector3Int(-10, -5, 0));

        var camera = Camera.main.GetOrAddComponent<CameraController>();
        camera.Target = hero;

        Managers.UI.ShowBaseUI<UI_Joystick>();

        {
            var monster = Managers.Object.Spawn<Monster>(new Vector3Int(0, 1, 0));
            monster.CreatureState = ECreatureState.Idle;
        }

        return true;
    }

    public override void Clear()
    {

    }
}
