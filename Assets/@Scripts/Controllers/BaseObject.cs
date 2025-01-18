using Spine.Unity;
using UnityEngine;
using static Define;

public class BaseObject : InitBase
{
    bool lookLeft = true;
    public EObjectType ObjectType { get; protected set; } = EObjectType.None;
    public CircleCollider2D Collider { get; private set; }
    public SkeletonAnimation SkeletonAnim { get; private set; }
    public Rigidbody2D Rigidbody { get; protected set; }

    public float ColliderRadius
    {
        get { return Collider?.radius ?? 0.0f; }
    }
    public Vector3 CenterPosition
    {
        get { return transform.position + Vector3.up * ColliderRadius; }
    }

    public bool LookLeft
    {
        get { return lookLeft; }
        set
        {
            lookLeft = value;
            Flip(!value);
        }
    }

    public override bool Init()
    {
        if (base.Init() == false) return false;

        Collider = gameObject.GetOrAddComponent<CircleCollider2D>();
        SkeletonAnim = GetComponent<SkeletonAnimation>();
        Rigidbody = GetComponent<Rigidbody2D>();
        return true;
    }

    #region Spine

    protected virtual void UpdateAnimation()
    {
    }

    public void PlayAnimation(int trackIndex, string AnimName, bool loop)
    {
        if (SkeletonAnim == null)
            return;

        SkeletonAnim.AnimationState.SetAnimation(trackIndex, AnimName, loop);
    }

    public void AddAnimation(int trackIndex, string AnimName, bool loop, float delay)
    {
        if (SkeletonAnim == null)
            return;

        SkeletonAnim.AnimationState.AddAnimation(trackIndex, AnimName, loop, delay);
    }

    public void Flip(bool flag)
    {
        if (SkeletonAnim == null)
            return;

        SkeletonAnim.Skeleton.ScaleX = flag ? -1 : 1;
    }

    #endregion
}
