public static class Define
{
    public enum EColliderSize
    {
        Small,
        Normal,
        Big
    }

    public enum ECreatureState
    {
        None,
        Idle,
        Move,
        Skill,
        Dead
    }

    public enum ECreatureType
    {
        None,
        Hero,
        Monster,
        Npc
    }

    public enum EEnvState
    {
        Idle,
        OnDamaged,
        Dead
    }

    public enum EHeroMoveState
    {
        None,
        TargetMonster,
        CollectEnv,
        ReturnToCamp,
        ForceMove
    }

    public enum EJoystickState
    {
        PointerDown,
        PointerUp,
        Drag
    }

    public enum ELayer
    {
        Default = 0,
        TransparentFX = 1,
        IgnoreRaycast = 2,
        Dummy1 = 3,
        Water = 4,
        UI = 5,
        Hero = 6,
        Monster = 7,
        Env = 8,
        Obstacle = 9,
        Projectile = 10
    }

    public enum EObjectType
    {
        None,
        HeroCamp,
        Creature,
        Projectile,
        Env
    }

    public enum EScene
    {
        Unknown,
        TitleScene,
        GameScene
    }

    public enum ESound
    {
        Bgm,
        Effect,
        Max
    }

    public enum EUIEvent
    {
        Click,
        PointerDown,
        PointerUp,
        Drag
    }

    public const int CAMERA_PROJECTION_SIZE = 12;

    public const int HERO_WIZARD_ID = 201000;
    public const int HERO_KNIGHT_ID = 201001;

    public const int MONSTER_SLIME_ID = 202001;
    public const int MONSTER_SPIDER_COMMON_ID = 202002;
    public const int MONSTER_WOOD_COMMON_ID = 202004;
    public const int MONSTER_GOBLIN_ARCHER_ID = 202005;
    public const int MONSTER_BEAR_ID = 202006;

    public const int ENV_TREE1_ID = 300001;
    public const int ENV_TREE2_ID = 301000;
}

public static class AnimName
{
    public const string ATTACK_A = "attack";
    public const string ATTACK_B = "attack";
    public const string SKILL_A = "skill";
    public const string SKILL_B = "skill";
    public const string IDLE = "idle";
    public const string MOVE = "move";
    public const string DAMAGED = "hit";
    public const string DEAD = "dead";
    public const string EVENT_ATTACK_A = "event_attack";
    public const string EVENT_ATTACK_B = "event_attack";
    public const string EVENT_SKILL_A = "event_attack";
    public const string EVENT_SKILL_B = "event_attack";
}

public static class SortingLayers
{
    public const int SPELL_INDICATOR = 200;
    public const int CREATURE = 300;
    public const int ENV = 300;
    public const int PROJECTILE = 310;
    public const int SKILL_EFFECT = 310;
    public const int DAMAGE_FONT = 410;
}
