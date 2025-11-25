using Managers;
using ScriptableVariables.Enums;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager _manager;

    [SerializeField] private GameObject MainMenuContainer;
    [SerializeField] private GameObject SettingsMenuContainer;
    [SerializeField] private bool debugMode;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private PlayerStateVariable playerStateVariable;

    private const string StartScene = "Village";

    public enum MainMenuButtons
    {
        StartGame,
        Unpause,
        Settings,
        Quit,
        Audio,
        Back
    };

    // Singleton access to static instance
    public void Awake()
    {
        //Application.targetFrameRate = 144;
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
        this.DebugMessage("Button clicked: " + buttonClicked.ToString());
        switch (buttonClicked)
        {
            case MainMenuButtons.StartGame:
                this.StartGame();
                break;
            case MainMenuButtons.Unpause:
                this.UnpauseGame();
                break;
            case MainMenuButtons.Settings:
                this.OpenSettings();
                break;
            case MainMenuButtons.Audio:
                //TODO: Implement audio function
                break;
            case MainMenuButtons.Back:
                this.BackToMainMenu();
                break;
            case MainMenuButtons.Quit:
                this.QuitGame();
                break;
            default:
                Debug.Log("Unknown Menu Button");
                break;
        }
    }

    private void DebugMessage(string message)
    {
        if (this.debugMode)
        {
            Debug.Log(message);
        }
    }

    private void StartGame()
    {
        if (StartScene != null)
            SceneManager.LoadScene(StartScene);
    }

    private void UnpauseGame()
    {
        Time.timeScale = 1;
        this.uiManager.SetCanvas(UIManager.CanvasType.Pause, false);
        this.uiManager.SetCanvas(UIManager.CanvasType.Player, true);
        this.uiManager.SetCanvas(UIManager.CanvasType.Minimap, true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        this.playerStateVariable.Value = PlayerStateEnum.Free;
    }

    private void OpenSettings()
    {
        this.MainMenuContainer.SetActive(false);
        this.SettingsMenuContainer.SetActive(true);
    }

    private void BackToMainMenu()
    {
        this.SettingsMenuContainer.SetActive(false);
        this.MainMenuContainer.SetActive(true);
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
    }
}