using UnityEngine;
using System.Collections.Generic;

public class StrokeTraceValidator : MonoBehaviour
{
    [Header("Reference Data")]
    public LetterStrokeData letterData;

    [Header("Drawing")]
    public LineRenderer linePrefab;
    public Transform lineParent;

    [Header("Scoring")]
    public float tolerance = 40f;
    public float minDrawDistance = 5f;
    [Header("Stroke Filter")]
    public float minStrokeLength = 50f; // px
    public int minStrokePoints = 5;
    [Header("Submit Panel")]
    public SubmitPanelController submitPanel;

    private Camera cam;
    private LineRenderer currentLine;
    private Vector2 lastDrawPos;
    private bool isDrawing;

    // 🔥 User drawn strokes
    private List<List<Vector2>> userStrokes = new();

    void Start()
    {
        cam = Camera.main;

        if (SelectedLetterHolder.current != null)
            letterData = SelectedLetterHolder.current.strokeData;
    }

    void Update()
    {
        if (letterData == null) return;

#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouse();
#else
        HandleTouch();
#endif
    }

    #region Input

    void HandleMouse()
    {
        if (Input.GetMouseButtonDown(0))
            Begin(Input.mousePosition);
        else if (Input.GetMouseButton(0))
            Move(Input.mousePosition);
        else if (Input.GetMouseButtonUp(0))
            End();
    }

    void HandleTouch()
    {
        if (Input.touchCount == 0) return;
        Touch t = Input.GetTouch(0);

        if (t.phase == TouchPhase.Began)
            Begin(t.position);
        else if (t.phase == TouchPhase.Moved || t.phase == TouchPhase.Stationary)
            Move(t.position);
        else if (t.phase == TouchPhase.Ended)
            End();
    }

    #endregion

    void Begin(Vector2 screenPos)
    {
        currentLine = Instantiate(linePrefab, lineParent);
        currentLine.positionCount = 0;

        userStrokes.Add(new List<Vector2>());
        userStrokes[^1].Add(screenPos);

        lastDrawPos = screenPos;
        isDrawing = true;

        AddDrawPoint(screenPos);
    }

    void Move(Vector2 screenPos)
    {
        if (!isDrawing) return;

        if (Vector2.Distance(lastDrawPos, screenPos) < minDrawDistance)
            return;

        userStrokes[^1].Add(screenPos);
        AddDrawPoint(screenPos);
    }

    void End()
    {
        if (!isDrawing) return;

        isDrawing = false;

        var currentStroke = userStrokes[^1];

        // ❌ TOO SMALL → DELETE
        if (currentStroke.Count < minStrokePoints ||
            CalculateStrokeLength(currentStroke) < minStrokeLength)
        {
            // Remove stored stroke
            userStrokes.RemoveAt(userStrokes.Count - 1);

            // Remove visual line
            Destroy(currentLine.gameObject);

            Debug.Log("❌ Tiny stroke ignored");
        }

        currentLine = null;
    }

    #region Drawing

    void AddDrawPoint(Vector2 screenPos)
    {
        Vector3 worldPos = cam.ScreenToWorldPoint(
            new Vector3(screenPos.x, screenPos.y, 10f)
        );

        currentLine.positionCount++;
        currentLine.SetPosition(
            currentLine.positionCount - 1,
            worldPos
        );

        lastDrawPos = screenPos;
    }

    #endregion

    // 🚀 SUBMIT BUTTON CALLS THIS
    public void SubmitLetter()
    {
        if (letterData == null) return;

        int strokeCount = Mathf.Min(
            letterData.strokes.Count,
            userStrokes.Count
        );

        float totalCoverage = 0f;

        for (int i = 0; i < strokeCount; i++)
        {
            // 1️⃣ Simplify reference stroke
            var reference = StrokeSimplifier.Simplify(
                letterData.strokes[i].screenPoints,
                20f // 🔥 VERY IMPORTANT (tune this)
            );

            // 2️⃣ Measure coverage using line area
            float coverage = StrokeCoverageUtility.CalculateCoverage(
                reference,
                userStrokes[i],
                tolerance // acts as brush radius
            );

            totalCoverage += coverage;

            Debug.Log($"Stroke {i + 1} Coverage: {coverage:P0}");
        }

        float avgCoverage = totalCoverage / strokeCount;
        int stars = CoverageToStars(avgCoverage);

        Debug.Log($"FINAL Coverage: {avgCoverage:P0}");
        Debug.Log($"⭐ Stars: {stars}");

        SaveResult(stars);
        ClearAllStrokes();
        submitPanel.Show(
      stars,
      avgCoverage,
      this
  );
        AdMobManager.Instance.TryShowInterstitial();
        //UIManager.Instance.ShowPanel(PanelType.LevelPage);
    }
    int CoverageToStars(float coverage)
    {
        if (coverage >= 0.70f) return 3;
        if (coverage >= 0.30f) return 2;
        if (coverage >= 0.20f) return 1;
        return 0;
    }


    int CountCoveredPoints(List<Vector2> reference, List<Vector2> user)
    {
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
        return covered;
    }
    float CalculateStrokeLength(List<Vector2> stroke)
    {
        float length = 0f;

        for (int i = 1; i < stroke.Count; i++)
        {
            length += Vector2.Distance(stroke[i - 1], stroke[i]);
        }

        return length;
    }


    int CalculateStars(float percent)
    {
        if (percent >= 0.9f) return 3;
        if (percent >= 0.7f) return 2;
        if (percent >= 0.4f) return 1;
        return 0;
    }
    public void ClearAllStrokes()
    {
        // Remove all drawn lines
        foreach (Transform child in lineParent)
        {
            Destroy(child.gameObject);
        }

        // Clear stored stroke data
        userStrokes.Clear();

        // Reset state
        isDrawing = false;
        currentLine = null;
        lastDrawPos = Vector2.zero;

        Debug.Log("🧹 All strokes cleared");
    }

    void SaveResult(int stars)
    {
        var letter = SelectedLetterHolder.current;
        var db = LetterProgressDatabaseHolder.Instance;

        if (letter == null || db == null) return;

        letter.starsEarned = Mathf.Max(letter.starsEarned, stars);

        int index = db.letters.IndexOf(letter);
        if (index >= 0 && index + 1 < db.letters.Count)
            db.letters[index + 1].unlocked = true;

        LetterProgressSaveSystem.Save(db);
    }
}
