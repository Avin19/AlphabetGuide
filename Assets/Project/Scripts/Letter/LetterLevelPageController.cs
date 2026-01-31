using System;
using UnityEngine;
using UnityEngine.UI;

public class LetterLevelPageController : MonoBehaviour
{
    [SerializeField] private LetterProgressDatabase database;
    [SerializeField] private LetterButtonUI buttonPrefab;
    [SerializeField] private Transform gridParent;
    [SerializeField] private Button rewardUnlockedLetter;
    [SerializeField] private Button backButton;

    void OnEnable()
    {
        LetterProgressSaveSystem.Load(database);
        BuildUI();
        rewardUnlockedLetter.onClick.AddListener(() => RewardUnlockedLetter());
        backButton.onClick.AddListener(() => BackButton());

    }

    private void BackButton()
    {
        UIManager.Instance.ShowPanel(PanelType.MainMenu);
    }

    void OnDisable()
    {
        rewardUnlockedLetter.onClick.RemoveAllListeners();
        backButton.onClick.RemoveAllListeners();
    }

    private void RewardUnlockedLetter()
    {
        AdMobManager.Instance.ShowRewarded(() =>
    {
        var letter = SelectedLetterHolder.current;
        if (letter == null) return;
        UnlockNextLetter();

        LetterProgressSaveSystem.Save(
            LetterProgressDatabaseHolder.Instance
        );

    });

    }

    void BuildUI()
    {
        foreach (Transform child in gridParent)
            Destroy(child.gameObject);

        foreach (var letter in database.letters)
        {
            var btn = Instantiate(buttonPrefab, gridParent);
            btn.Setup(letter, OnLetterSelected);
        }
    }

    void OnLetterSelected(LetterProgress progress)
    {
        SelectedLetterHolder.current = progress;

        UIManager.Instance.ShowPanel(PanelType.LetterPanel);
    }
    void UnlockNextLetter()
    {
        var db = LetterProgressDatabaseHolder.Instance;
        var current = SelectedLetterHolder.current;

        if (db == null || current == null)
            return;

        int index = db.letters.IndexOf(current);

        if (index < 0 || index + 1 >= db.letters.Count)
            return;

        var next = db.letters[index + 1];

        if (!next.unlocked)
        {
            next.unlocked = true;
            LetterProgressSaveSystem.Save(db);


        }
    }

}
