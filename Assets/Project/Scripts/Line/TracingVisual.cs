using UnityEngine;
using System.Collections.Generic;

public class TracingVisual : MonoBehaviour
{
    [Header("Line Settings")]
    public LineRenderer linePrefab;
    [Tooltip("Parent object for all stroke lines (must NOT be DontDestroyOnLoad)")]
    public Transform lineParent;

    LineRenderer currentLine;
    List<LineRenderer> lines = new List<LineRenderer>();

    public void StartNewLine()
    {
        if (lineParent == null)
        {
            Debug.LogError("Line Parent is NULL!");
            return;
        }

        // 🔥 IMPORTANT: instantiate WITHOUT parent first
        currentLine = Instantiate(linePrefab);

        // THEN parent it safely
        currentLine.transform.SetParent(lineParent, false);

        currentLine.positionCount = 0;
        lines.Add(currentLine);
    }


    public void AddPoint(Vector3 worldPos)
    {
        if (!currentLine)
            StartNewLine();

        currentLine.positionCount++;
        currentLine.SetPosition(
            currentLine.positionCount - 1,
            worldPos
        );
    }

    public void FinishCurrentLine()
    {
        currentLine = null;
    }
    public int CurrentLinePointCount
    {
        get
        {
            if (currentLine == null)
                return 0;

            return currentLine.positionCount;
        }
    }

    public void ClearAllLines()
    {
        foreach (var l in lines)
            Destroy(l.gameObject);

        lines.Clear();
        currentLine = null;
    }
}
