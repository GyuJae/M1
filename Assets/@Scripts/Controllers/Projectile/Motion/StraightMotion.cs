﻿using System;
using System.Collections;
using UnityEngine;

public class StraightMotion : ProjectileMotionBase
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    public new void SetInfo(int dataTemplateID, Vector3 startPosition, Vector3 targetPosition, Action endCallback)
    {
        base.SetInfo(dataTemplateID, startPosition, targetPosition, endCallback);
    }

    protected override IEnumerator CoLaunchProjectile()
    {
        var journeyLength = Vector3.Distance(StartPosition, TargetPosition);
        var totalTime = journeyLength / ProjectileData.ProjSpeed;
        float elapsedTime = 0;

        while (elapsedTime < totalTime)
        {
            elapsedTime += Time.deltaTime;

            var normalizedTime = elapsedTime / totalTime;
            transform.position = Vector3.Lerp(StartPosition, TargetPosition, normalizedTime);

            if (LookAtTarget)
                LookAt2D(TargetPosition - transform.position);

            yield return null;
        }

        transform.position = TargetPosition;
        EndCallback?.Invoke();
    }
}
