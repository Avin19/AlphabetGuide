using UnityEngine;

public class DevModeStrokeRecorder : MonoBehaviour
{
    [Header("DEV MODE")]
    public bool devModeEnabled = true;

    [Header("Target Data")]
    public LetterStrokeData letterData;

    [Header("Settings")]
    public float recordMinDistance = 5f; // px

    private Stroke currentStroke;
    private Vector2 lastRecordedPos;
    private bool isDrawing;

    void Update()
    {
        if (!devModeEnabled || letterData == null)
            return;

#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouse();
#else
        HandleTouch();
#endif
    }

    void HandleMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartStroke(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0))
        {
            RecordPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            EndStroke();
        }
    }

    void HandleTouch()
    {
        if (Input.touchCount == 0) return;

        Touch t = Input.GetTouch(0);

        if (t.phase == TouchPhase.Began)
            StartStroke(t.position);

        else if (t.phase == TouchPhase.Moved || t.phase == TouchPhase.Stationary)
            RecordPoint(t.position);

        else if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
            EndStroke();
    }

    void StartStroke(Vector2 screenPos)
    {
        currentStroke = new Stroke();
        letterData.strokes.Add(currentStroke);

        currentStroke.screenPoints.Add(screenPos);
        lastRecordedPos = screenPos;
        isDrawing = true;
    }

    void RecordPoint(Vector2 screenPos)
    {
        if (!isDrawing) return;

        if (Vector2.Distance(lastRecordedPos, screenPos) >= recordMinDistance)
        {
            currentStroke.screenPoints.Add(screenPos);
            lastRecordedPos = screenPos;
        }
    }

    void EndStroke()
    {
        isDrawing = false;
        currentStroke = null;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (devModeEnabled && letterData != null)
        {
            UnityEditor.EditorUtility.SetDirty(letterData);
        }
    }
#endif
}
