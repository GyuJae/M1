using UnityEngine;

public class CameraController : InitBase
{
    public BaseObject Target { get; set; }

    void LateUpdate()
    {
        if (Target == null)
            return;

        var targetPosition = new Vector3(Target.CenterPosition.x, Target.CenterPosition.y, -10f);
        transform.position = targetPosition;
    }

    public override bool Init()
    {
        if (base.Init() == false) return false;

        Camera.main.orthographicSize = 15.0f;
        return true;
    }
}
