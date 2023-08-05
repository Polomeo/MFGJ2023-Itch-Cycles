using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private TextMeshProUGUI interactionText;
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private GameObject gameWinCanvas;

    // SINGLETON
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        interactionText.gameObject.SetActive(false);
        gameOverCanvas.SetActive(false);
        gameWinCanvas.SetActive(false);
    }

    public void DisplayInteractionText(string text)
    {
        if(GameManager.Instance.isGameActive)
        {
            ActivateText();
            interactionText.text = text;
        }
    }

    public void UpdateInteractionText(string text) 
    {
        interactionText.SetText(text);
    }
    public void ActivateText()
    {
        interactionText.gameObject.SetActive(true);
    }
    public void HideText()
    {
        interactionText.gameObject.SetActive(false);
    }

    public void ShowGameOverCanvas()
    {
        gameOverCanvas.SetActive(true);
    }

    public void ShowGameWinCanvas()
    {
        gameWinCanvas.SetActive(true);
        gameWinCanvas.GetComponentInChildren<UISpriteAnimator>().PlayUIAnim();
    }
}
