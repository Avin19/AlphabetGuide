using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject levelPagePanel;
    [SerializeField] private GameObject letterPanel;

    [Header("Buttons")]
    [SerializeField] private Button letterBtn;

    public static UIManager Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ShowPanel(PanelType.MainMenu);
    }

    void OnEnable()
    {
        letterBtn.onClick.AddListener(OpenLevelPage);
    }

    void OnDisable()
    {
        letterBtn.onClick.RemoveAllListeners();
    }

    public void ShowPanel(PanelType panel)
    {
        mainMenuPanel.SetActive(false);
        levelPagePanel.SetActive(false);
        letterPanel.SetActive(false);

        switch (panel)
        {
            case PanelType.MainMenu:
                mainMenuPanel.SetActive(true);
                break;

            case PanelType.LevelPage:
                levelPagePanel.SetActive(true);
                break;

            case PanelType.LetterPanel:
                letterPanel.SetActive(true);
                break;
        }
    }

    void OpenLevelPage()
    {
        ShowPanel(PanelType.LevelPage);
    }
}
public enum PanelType
{
    None,
    MainMenu,
    LevelPage,
    LetterPanel
}

