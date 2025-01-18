using UnityEngine;
using static Define;

public class Monster : Creature
{
    Vector3 destPos;
    Vector3 initPos;
    Creature target;
    public float SearchDistance { get; } = 8.0f;
    public float AttackDistance { get; } = 4.0f;

    public override bool Init()
    {
        if (base.Init() == false) return false;

        CreatureType = ECreatureType.Monster;
        CreatureState = ECreatureState.Idle;
        Speed = 3.0f;

        StartCoroutine(CoUpdateAI());

        return true;
    }

    #region AI

    protected override void UpdateIdle()
    {
        Debug.Log("Idle");

        // Patrol
        {
            var patrolPercent = 10;
            var rand = Random.Range(0, 100);
            if (rand <= patrolPercent)
            {
                destPos = initPos + new Vector3(Random.Range(-2, 2), Random.Range(-2, 2));
                CreatureState = ECreatureState.Move;
                return;
            }
        }

        // Search Player
        {
            Creature target = null;
            var bestDistanceSqr = float.MaxValue;
            var searchDistanceSqr = SearchDistance * SearchDistance;

            foreach (var hero in Managers.Object.Heroes)
            {
                var dir = hero.transform.position - transform.position;
                var distToTargetSqr = dir.sqrMagnitude;

                Debug.Log(distToTargetSqr);

                if (distToTargetSqr > searchDistanceSqr)
                    continue;

                if (distToTargetSqr > bestDistanceSqr)
                    continue;

                target = hero;
                bestDistanceSqr = distToTargetSqr;
            }

            this.target = target;

            if (this.target != null)
                CreatureState = ECreatureState.Move;
        }
    }

    protected override void UpdateMove()
    {
        Debug.Log("Move");

        if (target == null)
        {
            // Patrol or Return
            var dir = destPos - transform.position;
            var moveDist = Mathf.Min(dir.magnitude, Time.deltaTime * Speed);
            transform.TranslateEx(dir.normalized * moveDist);

            if (dir.sqrMagnitude <= 0.01f)
            {
                CreatureState = ECreatureState.Idle;
            }
        }
        else
        {
            // Chase
            var dir = target.transform.position - transform.position;
            var distToTargetSqr = dir.sqrMagnitude;
            var attackDistanceSqr = AttackDistance * AttackDistance;

            if (distToTargetSqr < attackDistanceSqr)
            {
                // 공격 범위 이내로 들어왔으면 공격.
                CreatureState = ECreatureState.Skill;
                StartWait(2.0f);
            }
            else
            {
                // 공격 범위 밖이라면 추적.
                var moveDist = Mathf.Min(dir.magnitude, Time.deltaTime * Speed);
                transform.TranslateEx(dir.normalized * moveDist);

                // 너무 멀어지면 포기.
                var searchDistanceSqr = SearchDistance * SearchDistance;
                if (distToTargetSqr > searchDistanceSqr)
                {
                    destPos = initPos;
                    target = null;
                    CreatureState = ECreatureState.Move;
                }
            }
        }
    }

    protected override void UpdateSkill()
    {
        Debug.Log("Skill");

        if (_coWait != null)
            return;

        CreatureState = ECreatureState.Move;
    }

    protected override void UpdateDead()
    {
        Debug.Log("Dead");

    }

    #endregion
}
