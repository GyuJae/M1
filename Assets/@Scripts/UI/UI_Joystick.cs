using UnityEngine;
using UnityEngine.EventSystems;
using static Define;

public class UI_Joystick : UI_Base
{
    GameObject background;
    GameObject cursor;
    float radius;
    Vector2 touchPos;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObjects(typeof(GameObjects));

        background = GetObject((int)GameObjects.JoystickBG);
        cursor = GetObject((int)GameObjects.JoystickCursor);
        radius = background.GetComponent<RectTransform>().sizeDelta.y / 5;

        gameObject.BindEvent(OnPointerDown, EUIEvent.PointerDown);
        gameObject.BindEvent(OnPointerUp, EUIEvent.PointerUp);
        gameObject.BindEvent(OnDrag, EUIEvent.Drag);

        return true;
    }

    enum GameObjects
    {
        JoystickBG,
        JoystickCursor
    }


    #region Event

    public void OnPointerDown(PointerEventData eventData)
    {
        background.transform.position = eventData.position;
        cursor.transform.position = eventData.position;
        touchPos = eventData.position;

        Managers.Game.JoystickState = EJoystickState.PointerDown;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        cursor.transform.position = touchPos;

        Managers.Game.MoveDir = Vector2.zero;
        Managers.Game.JoystickState = EJoystickState.PointerUp;
    }

    public void OnDrag(PointerEventData eventData)
    {
        var touchDir = eventData.position - touchPos;

        var moveDist = Mathf.Min(touchDir.magnitude, radius);
        var moveDir = touchDir.normalized;
        var newPosition = touchPos + moveDir * moveDist;
        cursor.transform.position = newPosition;

        Managers.Game.MoveDir = moveDir;
        Managers.Game.JoystickState = EJoystickState.Drag;
    }

    #endregion
}
