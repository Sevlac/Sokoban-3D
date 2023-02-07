using System;
using System.Collections;
using UnityEngine;

public class Translate : Move
{
    [SerializeField]
    private float _rollSpeedDegPerSec = 270;
    private float _RotationAngle;

    protected override IEnumerator InternalMove(Vector3 dir)
    {
        float remainingAngle = 90;
        Vector3 totalDistance = new Vector3(0, 0, 0);

        while (remainingAngle > 0)
        {
            _RotationAngle = MathF.Min(_rollSpeedDegPerSec * Time.deltaTime, remainingAngle);
            double radian = Math.PI * remainingAngle / 180f;
            Vector3 newDistance = dir * MathF.Cos(Convert.ToSingle(radian)) - totalDistance;
            totalDistance += newDistance;
            transform.position += newDistance;
            remainingAngle -= _RotationAngle;
            yield return null;
        }
    }
}
