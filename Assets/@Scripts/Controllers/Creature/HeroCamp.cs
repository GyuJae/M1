using UnityEngine;
using static Define;

public class HeroCamp : BaseObject
{
    Vector2 _moveDir = Vector2.zero;

    public float Speed { get; set; } = 5.0f;

    public Transform Pivot { get; private set; }
    public Transform Destination { get; private set; }

    void Update()
    {
        transform.Translate(_moveDir * (Time.deltaTime * Speed));
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Managers.Game.OnMoveDirChanged -= HandleOnMoveDirChanged;
        Managers.Game.OnMoveDirChanged += HandleOnMoveDirChanged;

        Collider.includeLayers = 1 << (int)ELayer.Obstacle;
        Collider.excludeLayers = 1 << (int)ELayer.Monster | 1 << (int)ELayer.Hero;

        ObjectType = EObjectType.HeroCamp;

        Pivot = Util.FindChild<Transform>(gameObject, "Pivot", true);
        Destination = Util.FindChild<Transform>(gameObject, "Destination", true);

        return true;
    }

    void HandleOnMoveDirChanged(Vector2 dir)
    {
        _moveDir = dir;

        if (dir != Vector2.zero)
        {
            var angle = Mathf.Atan2(-dir.x, +dir.y) * 180 / Mathf.PI;
            Pivot.eulerAngles = new Vector3(0, 0, angle);
        }
    }
}
