using System.Collections.Generic;
using UnityEngine;

public static class StrokeCoverageUtility
{
    public static float CalculateCoverage(
        List<Vector2> reference,
        List<Vector2> userStroke,
        float brushRadius
    )
    {
        if (reference == null || userStroke == null)
            return 0f;

        int covered = 0;

        foreach (var refPoint in reference)
        {
            if (IsPointCovered(refPoint, userStroke, brushRadius))
                covered++;
        }

        return (float)covered / reference.Count;
    }

    static bool IsPointCovered(
        Vector2 point,
        List<Vector2> stroke,
        float radius
    )
    {
        for (int i = 0; i < stroke.Count - 1; i++)
        {
            if (DistancePointToSegment(point, stroke[i], stroke[i + 1]) <= radius)
                return true;
        }
        return false;
    }

    static float DistancePointToSegment(Vector2 p, Vector2 a, Vector2 b)
    {
        Vector2 ab = b - a;
        float t = Vector2.Dot(p - a, ab) / ab.sqrMagnitude;
        t = Mathf.Clamp01(t);
        Vector2 closest = a + ab * t;
        return Vector2.Distance(p, closest);
    }
}
