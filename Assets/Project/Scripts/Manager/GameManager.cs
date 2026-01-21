using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("Letter Data")]
    public List<LetterTraceData> letters;

    [Header("References")]
    public LetterPanelView letterPanel;
    public LetterTracingController tracingController;

    int currentIndex = 0;

    void Start()
    {
        if (letters.Count == 0)
        {
            Debug.LogError("No letters assigned!");
            return;
        }

        // Subscribe to tracing completion
        tracingController.LetterCompleted += LoadNextLetter;

        // Load first letter
        LoadLetter(currentIndex);
    }

    void LoadLetter(int index)
    {
        if (index < 0 || index >= letters.Count)
        {
            Debug.Log("All letters completed!");
            return;
        }

        LetterTraceData data = letters[index];

        letterPanel.ShowLetter(data);
        tracingController.ResetTracing(data); // optional but recommended

        Debug.Log("Loaded letter: " + data.letter);
    }

    void LoadNextLetter()
    {
        currentIndex++;

        if (currentIndex >= letters.Count)
        {
            OnAllLettersCompleted();
            return;
        }

        LoadLetter(currentIndex);
    }

    void OnAllLettersCompleted()
    {
        Debug.Log("🎉 All letters completed!");
        // Show rewards / next level / menu
    }
}
