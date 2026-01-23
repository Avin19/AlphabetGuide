using UnityEngine;

public class LetterProgressDatabaseHolder : MonoBehaviour
{
    public static LetterProgressDatabase Instance;

    public LetterProgressDatabase database;

    void Awake()
    {
        Instance = database;
    }
}
