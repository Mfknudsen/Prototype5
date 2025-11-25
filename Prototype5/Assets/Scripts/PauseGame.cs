using Inventory;
using Managers;
using ScriptableVariables.Enums;
using UnityEngine;
using UnityEngine.EventSystems;

public sealed class PauseGame : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private InventoryHandler inventoryHandler;
    [SerializeField] private PlayerStateVariable playerStateVariable;

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
        if (this.playerStateVariable.Value != PlayerStateEnum.Free)
            return;

        this.playerStateVariable.Value = PlayerStateEnum.Paused;

        this.uiManager.SetCanvas(UIManager.CanvasType.Pause, true);
        this.uiManager.SetCanvas(UIManager.CanvasType.Player, false);
        this.uiManager.SetCanvas(UIManager.CanvasType.Minimap, false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;

        EventSystem.current?.SetSelectedGameObject(null);
        this.inventoryHandler.SkipNextPotionAttack();
    }
}