public sealed class GameSettings
{
    public static GameSettings Instance => instance ??= new GameSettings();

    private static GameSettings instance;

    private float gamepadTurnSpeed = 100;

    #region Getters

    public float GetGamepadTurnSpeed()
    {
        return this.gamepadTurnSpeed;
    }

    #endregion
}