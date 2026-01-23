using UnityEngine;

public class LetterLevelPageController : MonoBehaviour
{
    public LetterProgressDatabase database;
    public LetterButtonUI buttonPrefab;
    public Transform gridParent;

    void OnEnable()
    {
        LetterProgressSaveSystem.Load(database);
        BuildUI();
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
}
