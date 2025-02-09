using System;
using System.Collections;
using UnityEngine;

public class ParabolaMotion : ProjectileMotionBase
{
    public float HeightArc { get; protected set; } = 2;

    public new void SetInfo(int dataTemplateID, Vector3 startPosition, Vector3 targetPosition, Action endCallback)
    {
        base.SetInfo(dataTemplateID, startPosition, targetPosition, endCallback);
    }

    protected override IEnumerator CoLaunchProjectile()
    {
        var journeyLength = Vector2.Distance(StartPosition, TargetPosition);
        var totalTime = journeyLength / ProjectileData.ProjSpeed;
        float elapsedTime = 0;

        while (elapsedTime < totalTime)
        {
            elapsedTime += Time.deltaTime;

            var normalizedTime = elapsedTime / totalTime;

            // 포물선 모양으로 이동
            var x = Mathf.Lerp(StartPosition.x, TargetPosition.x, normalizedTime);
            var baseY = Mathf.Lerp(StartPosition.y, TargetPosition.y, normalizedTime);
            var arc = HeightArc * Mathf.Sin(normalizedTime * Mathf.PI);

            var y = baseY + arc;

            var nextPos = new Vector3(x, y);

            if (LookAtTarget)
                LookAt2D(nextPos - transform.position);

            transform.position = nextPos;

            yield return null;
        }

        EndCallback?.Invoke();
    }
}
