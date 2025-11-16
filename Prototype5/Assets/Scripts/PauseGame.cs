using Inventory;
using Managers;
using UnityEngine;
using UnityEngine.EventSystems;

public sealed class PauseGame : MonoBehaviour
{
    [SerializeField] private GameObject canvasTitleScreen;

    [SerializeField] private GameObject canvasPlayerScreen;

    [SerializeField] private InventoryHandler inventoryHandler;

    public GameObject GetCanvasPlayerScreen() => canvasPlayerScreen;
    
    private void OnEnable()
    {
        InputManager.Instance.EscapeEvent.AddListener(this.OnEscapeInput);
    }

    private void OnDisable()
    {
        InputManager.Instance.EscapeEvent.AddListener(this.OnEscapeInput);
    }

    private void OnEscapeInput()
    {
        canvasTitleScreen?.SetActive(true);
        canvasPlayerScreen?.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
        
        EventSystem.current?.SetSelectedGameObject(null);
        inventoryHandler.SkipNextPotionAttack();
    }
}