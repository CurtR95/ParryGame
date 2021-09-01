using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuHandler : MonoBehaviour
{
    public GameObject MainMenu, CreditsMenu, SettingsMenu;
    // Start is called before the first frame update
    void Start()
    {
        MainMenuInit();
    }

    public void MainMenuInit()
    {
        CreditsMenu.SetActive(false);
        SettingsMenu.SetActive(false);
        MainMenu.SetActive(true);
    }

     public void CreditsButton()
    {
        // Show Credits Menu
        MainMenu.SetActive(false);
        SettingsMenu.SetActive(false);
        CreditsMenu.SetActive(true);
    }

    public void SettingsButton()
    {
        // Show Credits Menu
        MainMenu.SetActive(false);
        CreditsMenu.SetActive(false);
        SettingsMenu.SetActive(true);
    }

    public void ControlScheme(string type) {
        PlayerPrefs.SetString("ControlScheme", type);
    }

    public void PlayButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Core Scene");
    }

    
    public void ExitButton()
    {
        // Quit Game
        Application.Quit();
    }
}
