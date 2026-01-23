using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LetterStrokeData", menuName = "Tracing/Letter Stroke Data")]
public class LetterStrokeData : ScriptableObject
{
    public Sprite letterSprite;
    public int letterIndex;
    public string letter;

    public List<Stroke> strokes = new List<Stroke>();
}

[System.Serializable]
public class Stroke
{
    public List<Vector2> screenPoints = new();
}
