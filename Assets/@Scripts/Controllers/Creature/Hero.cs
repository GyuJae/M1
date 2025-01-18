using System;
using UnityEngine;
using static Define;

public class Hero : Creature
{
    Vector2 moveDir = Vector2.zero;

    void Update()
    {
        transform.Translate(moveDir * (Speed * Time.deltaTime));
    }

    public override bool Init()
    {
        if (base.Init() == false) return false;

        CreatureType = ECreatureType.Hero;
        CreatureState = ECreatureState.Idle;
        Speed = 5.0f;

        Managers.Game.OnMoveDirChanged -= HandleOnMoveDirChanged;
        Managers.Game.OnMoveDirChanged += HandleOnMoveDirChanged;
        Managers.Game.OnJoystickStateChanged -= HandleOnJoystickStateChanged;
        Managers.Game.OnJoystickStateChanged += HandleOnJoystickStateChanged;

        return true;
    }

    void HandleOnMoveDirChanged(Vector2 dir)
    {
        moveDir = dir;
        Debug.Log(dir);
    }

    void HandleOnJoystickStateChanged(EJoystickState joystickState)
    {
        switch (joystickState)
        {
            case EJoystickState.PointerDown:
                CreatureState = ECreatureState.Move;
                break;
            case EJoystickState.Drag:
                break;
            case EJoystickState.PointerUp:
                CreatureState = ECreatureState.Idle;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(joystickState), joystickState, null);
        }
    }
}
