using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string START_SCENE = "Scenes/SampleScene";
    
    [SerializeField] private GameObject MainMenuContainer;
    [SerializeField] private GameObject SettingsMenuContainer;
    
    public void PlayGame()
    {

        Debug.Log("Clicked Play");
        SceneManager.LoadScene(START_SCENE);
    }

    public void OpenSettings()
    {
        Debug.Log("Clicked Settings");
        MainMenuContainer.SetActive(false);
        SettingsMenuContainer.SetActive(true);
    }

    public void BackToMainMenu()
    {
        Debug.Log("Clicked Back");
        SettingsMenuContainer.SetActive(false);
        MainMenuContainer.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Clicked Quit");
        Application.Quit();
    }
}
