using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tracing/Letter Progress Database")]
public class LetterProgressDatabase : ScriptableObject
{
    public List<LetterProgress> letters = new();
}

[System.Serializable]
public class LetterProgress
{
    public string letterId;              // "A", "अ", "Alif"
    public int starsEarned;               // 0–3
    public bool unlocked;
    public LetterStrokeData strokeData;   // reference data
}
