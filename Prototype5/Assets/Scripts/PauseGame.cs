using Inventory;
using Managers;
using UnityEngine;
using UnityEngine.EventSystems;

public sealed class PauseGame : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private InventoryHandler inventoryHandler;
    
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
        uiManager.SetCanvas(UIManager.CanvasType.Pause, true);
        uiManager.SetCanvas(UIManager.CanvasType.Player, false);
        uiManager.SetCanvas(UIManager.CanvasType.Minimap, false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
        
        EventSystem.current?.SetSelectedGameObject(null);
        inventoryHandler.SkipNextPotionAttack();
    }
}