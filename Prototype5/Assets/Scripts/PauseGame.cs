using System.Collections.Generic;
using Managers;
using ScriptableVariables.Objects;
using ScriptableVariables.SystemSpecific;
using UnityEngine;

public sealed class ResetGame : MonoBehaviour
{
    [SerializeField] private List<TransformVariable> transformVariables;

    [SerializeField] private InventoryItemListVariable inventoryItemListVariable;

    [SerializeField] private GameObject canvasTitleScreen;

    [SerializeField] private GameObject canvasPlayerScreen;

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

        foreach (TransformVariable transformVariable in this.transformVariables)
            transformVariable.Value = null;

        this.inventoryItemListVariable.Value?.Clear();
    }
}