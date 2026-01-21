
using UnityEngine;

[CreateAssetMenu(menuName = "Tracing/ letter Trace Data")]
public class LetterTraceData : ScriptableObject
{
    public Sprite letterSprite;
    public int letterIndex;
    public string letter;

    public StrokeData[] strokes;
}
[System.Serializable]
public class StrokeData
{
    public Vector2[] points;
}