using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject mainMenuButtons;
    [SerializeField] GameObject howToPlayMenu;
    [SerializeField] GameObject creditsMenu;

    // Start is called before the first frame update
    void Start()
    {
        // Initial state
        mainMenuButtons.SetActive(true);
        howToPlayMenu.SetActive(false);
        creditsMenu.SetActive(false);
    }

    public void StartButton()
    {
        SceneManager.LoadScene("Main");
    }

    public void HowToPlayButton()
    {
        mainMenuButtons.SetActive(false);
        howToPlayMenu.SetActive(true);
    }

    public void CredtisButton()
    {
        mainMenuButtons.SetActive(false);
        creditsMenu.SetActive(true);
    }

    public void BackFromHowToPlay()
    {
        howToPlayMenu.SetActive(false);
        mainMenuButtons.SetActive(true);
    }

    public void BackFromCredits()
    {
        creditsMenu.SetActive(false);
        mainMenuButtons.SetActive(true);
    }


    public void QuitButton()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }


}
