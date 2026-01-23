using System.Collections.Generic;
using UnityEngine;

public static class StrokeScoringUtility
{
    /// <summary>
    /// Scores a stroke purely based on how much of the reference stroke is covered.
    /// </summary>
    public static int ScoreStroke(
        List<Vector2> reference,
        List<Vector2> user,
        float tolerance
    )
    {
        if (reference == null || reference.Count == 0)
            return 1;

        if (user == null || user.Count < 3)
            return 1;

        int covered = 0;

        foreach (var refPoint in reference)
        {
            foreach (var userPoint in user)
            {
                if (Vector2.Distance(refPoint, userPoint) <= tolerance)
                {
                    covered++;
                    break;
                }
            }
        }

        float coverage = (float)covered / reference.Count;

        return CoverageToStrokeScore(coverage);
    }

    /// <summary>
    /// Converts coverage % to stroke score (1–5)
    /// </summary>
    private static int CoverageToStrokeScore(float coverage)
    {
        if (coverage >= 0.90f) return 5;
        if (coverage >= 0.75f) return 4;
        if (coverage >= 0.55f) return 3;
        if (coverage >= 0.35f) return 2;
        return 1;
    }

    /// <summary>
    /// Converts total coverage % to stars (0–3)
    /// </summary>
    public static int CoverageToStars(float coverage)
    {
        if (coverage >= 0.90f) return 3;
        if (coverage >= 0.70f) return 2;
        if (coverage >= 0.40f) return 1;
        return 0;
    }
}
