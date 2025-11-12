using System.Collections.Generic;
using Managers;
using ScriptableVariables.Objects;
using ScriptableVariables.SystemSpecific;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class ResetGame : MonoBehaviour
{
    [SerializeField] private List<TransformVariable> transformVariables;

    [SerializeField] private InventoryItemListVariable inventoryItemListVariable;

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
        SceneManager.LoadScene("Scenes/TitleScreen", LoadSceneMode.Single);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        foreach (TransformVariable transformVariable in this.transformVariables)
            transformVariable.Value = null;

        this.inventoryItemListVariable.Value.Clear();
    }
}