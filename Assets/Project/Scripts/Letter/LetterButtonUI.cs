using UnityEngine;
using UnityEngine.UI;

public class LetterButtonUI : MonoBehaviour
{
    [SerializeField] private Image letterSprite;
    [SerializeField] private Image[] stars;        // size = 3
    [SerializeField] private Sprite starSprite;
    [SerializeField] private Button button;

    private LetterProgress data;

    public void Setup(LetterProgress progress, System.Action<LetterProgress> onClick)
    {
        data = progress;
        letterSprite.sprite = data.strokeData.letterSprite;

        // Lock handling
        // lockIcon.SetActive(!progress.unlocked);
        button.interactable = progress.unlocked;

        UpdateStars(progress.starsEarned);

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onClick?.Invoke(progress));
    }

    private void UpdateStars(int starsEarned)
    {
        // Safety clamp (VERY IMPORTANT)
        starsEarned = Mathf.Clamp(starsEarned, 0, stars.Length);

        // Reset all stars (poor accuracy = 0 stars)
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].enabled = false;
        }

        // Fill earned stars
        for (int i = 0; i < starsEarned; i++)
        {
            stars[i].enabled = true;
        }
    }
}
