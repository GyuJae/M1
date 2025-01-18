using System;
using UnityEngine;
using static Define;

public class GameManager
{
    #region Hero

    Vector2 moveDir;
    public Vector2 MoveDir
    {
        get { return moveDir; }
        set
        {
            moveDir = value;
            OnMoveDirChanged?.Invoke(value);
        }
    }

    EJoystickState joystickState;
    public EJoystickState JoystickState
    {
        get { return joystickState; }
        set
        {
            joystickState = value;
            OnJoystickStateChanged?.Invoke(joystickState);
        }
    }

    #endregion

    #region Action

    public event Action<Vector2> OnMoveDirChanged;
    public event Action<EJoystickState> OnJoystickStateChanged;

    #endregion
}
