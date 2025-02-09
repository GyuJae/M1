using Spine;

public class NormalAttack : SkillBase
{
    public override void SetInfo(Creature owner, int skillTemplateID)
    {
        base.SetInfo(owner, skillTemplateID);
    }

    public override void DoSkill()
    {
        base.DoSkill();

        Owner.CreatureState = Define.ECreatureState.Skill;
        Owner.PlayAnimation(0, SkillData.AnimName, false);

        Owner.LookAtTarget(Owner.Target);
    }

    protected override void OnAnimEventHandler(TrackEntry trackEntry, Event e)
    {
        if (e.ToString().Contains(SkillData.AnimName))
            OnAttackEvent();
    }

    void PickupTargetAndProcessHit()
    {
    }

    protected virtual void OnAttackEvent()
    {
        if (Owner.Target.IsValid() == false)
            return;

        if (SkillData.ProjectileId == 0)
        {
            // Melee
            // Owner.Target.OnDamaged(Owner, this);
        }
        else
        {
            // Ranged
            GenerateProjectile(Owner, Owner.CenterPosition);
        }
    }

    protected override void OnAnimCompleteHandler(TrackEntry trackEntry)
    {
        // if (Owner.Target.IsValid() == false) return;

        if (Owner.CreatureState == Define.ECreatureState.Skill)
            Owner.CreatureState = Define.ECreatureState.Move;
    }
}
