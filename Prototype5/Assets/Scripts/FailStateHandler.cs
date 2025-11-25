using DayNightCycle;
using ScriptableVariables.Enums;
using UnityEngine;

public sealed class FailStateHandler : MonoBehaviour
{
    [SerializeField] private GameObject gameOverUI, playerCharacterUI, mapUI;
    [SerializeField] private PlayerStateVariable playerStateVariable;

    private void OnEnable()
    {
        DayNight.AddListener(this.OnDayTimeStateChange);
    }

    private void OnDisable()
    {
        DayNight.RemoveListener(this.OnDayTimeStateChange);
    }

    private void OnDayTimeStateChange(DayNightTime state)
    {
        if (state != DayNightTime.Morning)
            return;

        this.playerStateVariable.Value = PlayerStateEnum.InMenu;

        this.gameOverUI.SetActive(true);
        this.playerCharacterUI.SetActive(false);
        this.mapUI.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("Morning");
    }
}