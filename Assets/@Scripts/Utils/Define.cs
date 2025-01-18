public static class Define
{
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

    public enum EJoystickState
    {
        PointerDown,
        PointerUp,
        Drag
    }

    public enum EObjectType
    {
        None,
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
}

public static class AnimName
{
    public const string IDLE = "idle";
    public const string ATTACK_A = "attack_a";
    public const string ATTACK_B = "attack_b";
    public const string MOVE = "move";
    public const string DEAD = "dead";
}
