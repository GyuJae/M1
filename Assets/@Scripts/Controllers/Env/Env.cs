using Data;
using UnityEngine;
using static Define;

public class Env : BaseObject
{
    EEnvState _envState = EEnvState.Idle;
    EnvData data;
    public EEnvState EnvState
    {
        get { return _envState; }
        set
        {
            _envState = value;
            UpdateAnimation();
        }
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        ObjectType = EObjectType.Env;

        return true;
    }

    public void SetInfo(int templateID)
    {
        DataTemplateID = templateID;
        data = Managers.Data.EnvDic[templateID];

        // Stat
        Hp = data.MaxHp;
        MaxHp = data.MaxHp;

        // Spine
        var ranSpine = data.SkeletonDataIDs[Random.Range(0, data.SkeletonDataIDs.Count)];
        SetSpineAnimation(ranSpine, SortingLayers.ENV);
    }

    protected override void UpdateAnimation()
    {
        switch (EnvState)
        {
            case EEnvState.Idle:
                PlayAnimation(0, AnimName.IDLE, true);
                break;
            case EEnvState.OnDamaged:
                PlayAnimation(0, AnimName.DAMAGED, false);
                break;
            case EEnvState.Dead:
                PlayAnimation(0, AnimName.DEAD, false);
                break;
        }
    }

    public override void OnDamaged(BaseObject attacker, SkillBase skill)
    {
        if (EnvState == EEnvState.Dead)
            return;

        base.OnDamaged(attacker, skill);

        float finalDamage = 1;
        EnvState = EEnvState.OnDamaged;

        // TODO : Show UI

        Hp = Mathf.Clamp(Hp - finalDamage, 0, MaxHp);
        if (Hp <= 0)
            OnDead(attacker, skill);
    }

    public override void OnDead(BaseObject attacker, SkillBase skill)
    {
        base.OnDead(attacker, skill);

        EnvState = EEnvState.Dead;

        // TODO : Drop Item	

        Managers.Object.Despawn(this);
    }

    #region Stat

    public float Hp { get; set; }
    public float MaxHp { get; set; }

    #endregion
}
