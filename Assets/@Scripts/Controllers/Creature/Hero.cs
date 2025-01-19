using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Define;

public class Hero : Creature
{
    EHeroMoveState heroMoveState = EHeroMoveState.None;
    bool needArrange = true;
    public EHeroMoveState HeroMoveState
    {
        get { return heroMoveState; }
        private set
        {
            heroMoveState = value;
            NeedArrange = value switch
            {
                EHeroMoveState.CollectEnv => true,
                EHeroMoveState.TargetMonster => true,
                EHeroMoveState.ForceMove => true,
                _ => NeedArrange
            };
        }
    }
    public bool NeedArrange
    {
        get { return needArrange; }
        set
        {
            needArrange = value;

            if (value)
                ChangeColliderSize(EColliderSize.Big);
            else
                TryResizeCollider();
        }
    }

    public override bool Init()
    {
        if (base.Init() == false) return false;

        CreatureType = ECreatureType.Hero;

        Managers.Game.OnJoystickStateChanged -= HandleOnJoystickStateChanged;
        Managers.Game.OnJoystickStateChanged += HandleOnJoystickStateChanged;

        StartCoroutine(CoUpdateAI());

        return true;
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

    void TryResizeCollider()
    {
        // 일단 충돌체 아주 작게.
        ChangeColliderSize(EColliderSize.Small);

        if (Managers.Object.Heroes.Any(hero => hero.HeroMoveState == EHeroMoveState.ReturnToCamp))
        {
            return;
        }

        // ReturnToCamp가 한 명도 없으면 콜라이더 조정.

        foreach (var hero in Managers.Object.Heroes.Where(hero => hero.CreatureState == ECreatureState.Idle))
        {
            hero.ChangeColliderSize(EColliderSize.Big);
        }
    }

    BaseObject FindClosestInRange(float range, IEnumerable<BaseObject> objs)
    {
        BaseObject target = null;
        var bestDistanceSqr = float.MaxValue;
        var searchDistanceSqr = range * range;

        foreach (var obj in objs)
        {
            var dir = obj.transform.position - transform.position;
            var distToTargetSqr = dir.sqrMagnitude;

            // 서치 범위보다 멀리 있으면 스킵.
            if (distToTargetSqr > searchDistanceSqr)
                continue;

            // 이미 더 좋은 후보를 찾았으면 스킵.
            if (distToTargetSqr > bestDistanceSqr)
                continue;

            target = obj;
            bestDistanceSqr = distToTargetSqr;
        }

        return target;
    }

    void ChaseOrAttackTarget(float attackRange, float chaseRange)
    {
        var dir = _target.transform.position - transform.position;
        var distToTargetSqr = dir.sqrMagnitude;
        var attackDistanceSqr = attackRange * attackRange;

        if (distToTargetSqr <= attackDistanceSqr)
        {
            // 공격 범위 이내로 들어왔다면 공격.
            CreatureState = ECreatureState.Skill;
        }
        else
        {
            // 공격 범위 밖이라면 추적.
            SetRigidBodyVelocity(dir.normalized * MoveSpeed);

            // 너무 멀어지면 포기.
            var searchDistanceSqr = chaseRange * chaseRange;
            if (distToTargetSqr > searchDistanceSqr)
            {
                _target = null;
                HeroMoveState = EHeroMoveState.None;
                CreatureState = ECreatureState.Move;
            }
        }
    }

    #region AI

    public float SearchDistance { get; } = 8.0f;
    public float AttackDistance
    {
        get
        {
            var targetRadius = _target.IsValid() ? _target.ColliderRadius : 0;
            return ColliderRadius + targetRadius + 2.0f;
        }
    }

    public float StopDistance { get; } = 1.0f;
    BaseObject _target;
    public Transform HeroCampDest
    {
        get
        {
            var camp = Managers.Object.Camp;
            if (HeroMoveState == EHeroMoveState.ReturnToCamp)
                return camp.Pivot;

            return camp.Destination;
        }
    }


    protected override void UpdateIdle()
    {
        // 0. 이동 상태라면 강제 변경
        if (HeroMoveState == EHeroMoveState.ForceMove)
        {
            CreatureState = ECreatureState.Move;
            return;
        }

        // 0. 너무 멀어졌다면 강제로 이동

        // 1. 몬스터
        var creature = FindClosestInRange(SearchDistance, Managers.Object.Monsters) as Creature;
        if (creature != null)
        {
            _target = creature;
            CreatureState = ECreatureState.Move;
            HeroMoveState = EHeroMoveState.TargetMonster;
            return;
        }

        // 2. 주변 Env 채굴
        var env = FindClosestInRange(SearchDistance, Managers.Object.Envs) as Env;
        if (env != null)
        {
            _target = env;
            CreatureState = ECreatureState.Move;
            HeroMoveState = EHeroMoveState.CollectEnv;
            return;
        }

        // 3. Camp 주변으로 모이기
        if (NeedArrange)
        {
            CreatureState = ECreatureState.Move;
            HeroMoveState = EHeroMoveState.ReturnToCamp;
        }
    }

    protected override void UpdateMove()
    {
        // 0. 누르고 있다면, 강제 이동
        if (HeroMoveState == EHeroMoveState.ForceMove)
        {
            var dir = HeroCampDest.position - transform.position;
            SetRigidBodyVelocity(dir.normalized * MoveSpeed);
            return;
        }

        // 1. 주변 몬스터 서치
        if (HeroMoveState == EHeroMoveState.TargetMonster)
        {
            // 몬스터 죽었으면 포기.
            if (_target.IsValid() == false)
            {
                HeroMoveState = EHeroMoveState.None;
                CreatureState = ECreatureState.Move;
                return;
            }

            ChaseOrAttackTarget(AttackDistance, SearchDistance);
            return;
        }

        // 2. 주변 Env 채굴
        if (HeroMoveState == EHeroMoveState.CollectEnv)
        {
            // 몬스터가 있으면 포기.
            var creature = FindClosestInRange(SearchDistance, Managers.Object.Monsters) as Creature;
            if (creature != null)
            {
                _target = creature;
                HeroMoveState = EHeroMoveState.TargetMonster;
                CreatureState = ECreatureState.Move;
                return;
            }

            // Env 이미 채집했으면 포기.
            if (_target.IsValid() == false)
            {
                HeroMoveState = EHeroMoveState.None;
                CreatureState = ECreatureState.Move;
                return;
            }

            ChaseOrAttackTarget(AttackDistance, SearchDistance);
            return;
        }

        // 3. Camp 주변으로 모이기
        if (HeroMoveState == EHeroMoveState.ReturnToCamp)
        {
            var dir = HeroCampDest.position - transform.position;
            var stopDistanceSqr = StopDistance * StopDistance;
            if (dir.sqrMagnitude <= StopDistance)
            {
                HeroMoveState = EHeroMoveState.None;
                CreatureState = ECreatureState.Idle;
                NeedArrange = false;
                return;
            }
            // 멀리 있을 수록 빨라짐
            var ratio = Mathf.Min(1, dir.magnitude); // TEMP
            var moveSpeed = MoveSpeed * (float)Math.Pow(ratio, 3);
            SetRigidBodyVelocity(dir.normalized * moveSpeed);
            return;
        }

        // 4. 기타 (누르다 뗐을 때)
        CreatureState = ECreatureState.Idle;
    }

    protected override void UpdateSkill()
    {
        if (HeroMoveState == EHeroMoveState.ForceMove)
        {
            CreatureState = ECreatureState.Move;
            return;
        }

        if (_target.IsValid() == false)
        {
            CreatureState = ECreatureState.Move;
        }
    }

    protected override void UpdateDead()
    {
    }

    #endregion
}
