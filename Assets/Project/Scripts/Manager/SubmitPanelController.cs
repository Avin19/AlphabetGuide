using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SubmitPanelController : MonoBehaviour
{
    [Header("Canvas")]
    public CanvasGroup canvasGroup;

    [Header("Title Image")]
    public Image titleImage;

    [Header("Title Sprites")]
    public Sprite greatJobSprite;
    public Sprite goodTrySprite;
    public Sprite awesomeSprite;
    public Sprite wellDoneSprite;
    public Sprite letsPracticeSprite;

    [Header("Stars")]
    public Image[] stars;
    public Sprite filledStar;
    public Sprite emptyStar;

    [Header("Buttons")]
    public Button retryButton;
    public Button nextButton;
    public Button rewardButton;
    public TextMeshProUGUI coverageText;

    private StrokeTraceValidator traceValidator;

    void Awake()
    {
        Hide();
    }

    public void Show(
        int starsEarned,
        float coveragePercent,
        StrokeTraceValidator validator
    )
    {
        traceValidator = validator;

        UpdateTitle(starsEarned);
        UpdateStars(starsEarned);

        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;

        // Show rewarded only if useful
        coverageText.text = $"Coverage: {Mathf.RoundToInt(coveragePercent * 100)}%";
        rewardButton.gameObject.SetActive(
            starsEarned < 3 &&
            AdMobManager.Instance.IsRewardedReady()
        );
    }
    void OnEnable()
    {
        retryButton.onClick.AddListener(() => OnRetryClicked());
        nextButton.onClick.AddListener(() => OnNextClicked());
        rewardButton.onClick.AddListener(() => OnRewardClicked());

    }
    void Osable()
    {
        retryButton.onClick.RemoveAllListeners();
        nextButton.onClick.RemoveAllListeners();
        rewardButton.onClick.RemoveAllListeners();
    }
    void UpdateTitle(int starsEarned)
    {
        if (starsEarned == 3)
            titleImage.sprite = awesomeSprite;
        else if (starsEarned == 2)
            titleImage.sprite = greatJobSprite;
        else if (starsEarned == 1)
            titleImage.sprite = wellDoneSprite;
        else
            titleImage.sprite = letsPracticeSprite;
    }

    void UpdateStars(int starsEarned)
    {
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].sprite = i < starsEarned
                ? filledStar
                : emptyStar;
        }
    }

    public void Hide()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    // =========================
    // BUTTON CALLBACKS
    // =========================

    public void OnRetryClicked()
    {
        Hide();
        traceValidator.ClearAllStrokes();
    }

    public void OnNextClicked()
    {
        Hide();
        UIManager.Instance.ShowPanel(PanelType.LevelPage);
    }

    public void OnRewardClicked()
    {
        AdMobManager.Instance.ShowRewarded(() =>
        {
            var letter = SelectedLetterHolder.current;
            if (letter == null) return;

            letter.starsEarned =
                Mathf.Clamp(letter.starsEarned + 1, 0, 3);

            LetterProgressSaveSystem.Save(
                LetterProgressDatabaseHolder.Instance
            );

            UpdateStars(letter.starsEarned);
            UpdateTitle(letter.starsEarned);
        });
    }
}
