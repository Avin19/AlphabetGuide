using UnityEngine;

public static class LetterProgressSaveSystem
{
    public static void Save(LetterProgressDatabase db)
    {
        foreach (var l in db.letters)
        {
            PlayerPrefs.SetInt(l.letterId + "_stars", l.starsEarned);
            PlayerPrefs.SetInt(l.letterId + "_unlock", l.unlocked ? 1 : 0);
        }
        PlayerPrefs.Save();
    }

    public static void Load(LetterProgressDatabase db)
    {
        foreach (var l in db.letters)
        {
            l.starsEarned = PlayerPrefs.GetInt(l.letterId + "_stars", 0);
            l.unlocked = PlayerPrefs.GetInt(
                l.letterId + "_unlock",
                l.letterId == db.letters[0].letterId ? 1 : 0
            ) == 1;
        }
    }
}
