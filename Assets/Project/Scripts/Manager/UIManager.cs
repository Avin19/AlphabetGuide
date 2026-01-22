using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private Transform mainMenuPanel;
    [SerializeField] private Transform gamePanel;
    [Header("Button")]
    [SerializeField] private Button letterBtn;
    [SerializeField] private Button wordBtn;
    [SerializeField] private Button gameBtn;


    public void PanelController(PanelType _panel)
    {
        switch (_panel)
        {
            case PanelType.None:
                mainMenuPanel.gameObject.SetActive(false);
                gamePanel.gameObject.SetActive(false);
                break;
            case PanelType.MainMenu:
                mainMenuPanel.gameObject.SetActive(true);
                gamePanel.gameObject.SetActive(false);
                break;
            case PanelType.GamePanel:
                mainMenuPanel.gameObject.SetActive(false);
                gamePanel.gameObject.SetActive(true);
                break;


        }
    }
    void Start()
    {
        PanelController(PanelType.MainMenu);

    }
    void OnEnable()
    {
        letterBtn.onClick.AddListener(() => LetterPanel());
        wordBtn.onClick.AddListener(WordPanel());
        gameBtn.onClick.AddListener(GamePanel());
    }

    private UnityAction GamePanel()
    {
        throw new NotImplementedException();
    }

    private UnityAction WordPanel()
    {
        throw new NotImplementedException();
    }

    private UnityAction LetterPanel()
    {
        throw new NotImplementedException();
    }
}

public enum PanelType
{
    None,
    MainMenu,
    GamePanel
}