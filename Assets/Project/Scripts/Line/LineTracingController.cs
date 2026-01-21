using UnityEngine;
using System;

public class LetterTracingController : MonoBehaviour
{
    public Camera uiCamera;
    public RectTransform canvasRect;
    public TracingVisual visual;

    public float tolerance = 80f;

    LetterTraceData letterData;

    int currentStroke = 0;
    int currentPoint = 0;
    bool strokeFinishedPending = false;


    public Action LetterCompleted;

    void Update()
    {
        if (!Input.GetMouseButton(0) || letterData == null)
            return;

        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            Input.mousePosition,
            uiCamera,
            out localPos
        );

        StrokeData stroke = letterData.strokes[currentStroke];
        if (currentPoint >= stroke.points.Length)
            return;

        float dist = Vector2.Distance(localPos, stroke.points[currentPoint]);
        Debug.Log($"LocalPos: {localPos}, Target: {stroke.points[currentPoint]}");

        if (dist <= tolerance && !strokeFinishedPending)
        {
            Vector3 worldPoint = canvasRect.TransformPoint(localPos);
            visual.AddPoint(worldPoint);
            currentPoint++;

            // All logical points matched
            if (currentPoint >= stroke.points.Length)
            {
                strokeFinishedPending = true;
            }
        }
        if (strokeFinishedPending)
        {
            // Wait until line renderer has all points
            if (visual.CurrentLinePointCount >= letterData.strokes[currentStroke].points.Length)
            {
                strokeFinishedPending = false;
                NextStroke();
            }
        }


    }

    void NextStroke()
    {
        // ✅ FINALIZE CURRENT STROKE (DO NOT CLEAR)
        visual.FinishCurrentLine();

        currentStroke++;
        currentPoint = 0;

        if (currentStroke >= letterData.strokes.Length)
            CompleteLetter();
        else
            visual.StartNewLine(); // ✅ NEW LINE FOR NEXT STROKE
    }

    void CompleteLetter()
    {
        Debug.Log("✅ Letter Completed");
        LetterCompleted?.Invoke();
    }

    public void ResetTracing(LetterTraceData data)
    {
        letterData = data;
        currentStroke = 0;
        currentPoint = 0;

        if (visual != null)
            visual.ClearAllLines(); // ✅ FULL RESET ONLY HERE

        visual.StartNewLine();
    }

    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(
            canvasRect.TransformPoint(Vector3.zero),
            0.3f
        );
    }
}
