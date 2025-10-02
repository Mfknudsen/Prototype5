using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager _manager;
    [SerializeField] private string START_SCENE = "Scenes/SampleScene";
    [SerializeField] private GameObject MainMenuContainer;
    [SerializeField] private GameObject SettingsMenuContainer;
    [SerializeField] private bool debugMode;
    
    public enum MainMenuButtons
    {
        Play,
        Settings,
        Quit, 
        Audio, 
        Back
    };
    
    // Singleton access to static instance
    public void Awake()
    {
        if (_manager == null)
        {
            _manager = this;
        }
        else
        {
            Debug.LogError("There is already a MainMenuManager in the scene");
        }
        
    }

    public void MainMenuButtonClicked(MainMenuButtons buttonClicked)
    {
        DebugMessage("Button clicked: " + buttonClicked.ToString());
        switch (buttonClicked)
        {
            case MainMenuButtons.Play:
                PlayGame();
                break;
            case MainMenuButtons.Settings:
                OpenSettings();
                break;
            case MainMenuButtons.Audio:
                //TODO: Implement audio function
                break;
            case MainMenuButtons.Back:
                BackToMainMenu();
                break;
            case MainMenuButtons.Quit:
                QuitGame();
                break;
            default:
                Debug.Log("Unknown Menu Button");
                break;
        }
    }

    private void DebugMessage(string message)
    {
        if (debugMode)
        {
            Debug.Log(message);
        }
    }
    
    private void PlayGame()
    {
        SceneManager.LoadScene(START_SCENE);
    }

    private void OpenSettings()
    {
        MainMenuContainer.SetActive(false);
        SettingsMenuContainer.SetActive(true);
    }

    private void BackToMainMenu()
    {
        SettingsMenuContainer.SetActive(false);
        MainMenuContainer.SetActive(true);
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
