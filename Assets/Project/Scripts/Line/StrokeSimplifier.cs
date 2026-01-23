using System.Collections.Generic;
using UnityEngine;

public static class StrokeSimplifier
{
    /// <summary>
    /// Reduces stroke points to human-friendly milestones
    /// </summary>
    public static List<Vector2> Simplify(
        List<Vector2> points,
        float minDistance
    )
    {
        List<Vector2> result = new();

        if (points == null || points.Count == 0)
            return result;

        result.Add(points[0]);
        Vector2 last = points[0];

        for (int i = 1; i < points.Count; i++)
        {
            if (Vector2.Distance(points[i], last) >= minDistance)
            {
                result.Add(points[i]);
                last = points[i];
            }
        }

        return result;
    }
}
