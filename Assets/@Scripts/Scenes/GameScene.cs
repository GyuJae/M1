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

        for (var i = 0; i < 5; i++)
        {

            // var heroTemplateID = HERO_WIZARD_ID + Random.Range(0, 5);
            const int heroTemplateID = HERO_KNIGHT_ID;
            var hero = Managers.Object.Spawn<Hero>(
                new Vector3Int(-10 + Random.Range(-5, 5), -5 + Random.Range(-5, 5), 0), heroTemplateID);
        }
        var camp = Managers.Object.Spawn<HeroCamp>(new Vector3Int(-10, -5, 0), 0);

        var camera = Camera.main.GetOrAddComponent<CameraController>();
        camera.Target = camp;

        Managers.UI.ShowBaseUI<UI_Joystick>();

        /*
        {
            Managers.Object.Spawn<Monster>(new Vector3Int(0, 1, 0), MONSTER_BEAR_ID);
            Managers.Object.Spawn<Monster>(new Vector3Int(0, 1, 0), MONSTER_SLIME_ID);
            Managers.Object.Spawn<Monster>(new Vector3Int(0, 1, 0), MONSTER_GOBLIN_ARCHER_ID);
        }
        */

        {
            var env = Managers.Object.Spawn<Env>(new Vector3(0, 2, 0), ENV_TREE1_ID);
            env.EnvState = EEnvState.Idle;
        }

        return true;
    }

    public override void Clear()
    {

    }
}
