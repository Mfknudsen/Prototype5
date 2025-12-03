using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Managers
{
    public enum Device
    {
        Null,
        KeyboardAndMouse,
        Gamepad
    }

    public sealed class InputManager
    {
        public static InputManager Instance => _instance ??= new InputManager();

        private static InputManager _instance;

        private Device currentDevice;

        public readonly UnityEvent<Device>
            DeviceChangeEvent = new UnityEvent<Device>();

        public readonly UnityEvent<Vector2>
            MoveAxisInputEvent = new UnityEvent<Vector2>(),
            TurnAxisInputEvent = new UnityEvent<Vector2>(),
            ArrowAxisInputEvent = new UnityEvent<Vector2>();

        public readonly UnityEvent
            InteractInputEvent = new UnityEvent(),
            JumpInputEvent = new UnityEvent(),
            InventoryEvent = new UnityEvent(),
            ClickEvent = new UnityEvent(),
            EscapeEvent = new UnityEvent();

        public readonly UnityEvent<bool>
            RunInputEvent = new UnityEvent<bool>();

        public readonly UnityEvent<int>
            HotbarKey = new UnityEvent<int>();

        public readonly UnityEvent<bool>
            HotbarScroll = new UnityEvent<bool>();

        private InputManager()
        {
            this.currentDevice = Device.Null;

            InputSystem_Actions playerInput = new InputSystem_Actions();

            playerInput.Player.Enable();

            playerInput.Player.Move.performed +=
                context => this.MoveAxisInputEvent.Invoke(context.ReadValue<Vector2>());
            playerInput.Player.Move.canceled +=
                context => this.MoveAxisInputEvent.Invoke(context.ReadValue<Vector2>());

            playerInput.Player.Look.performed +=
                context =>
                {
                    this.OnInputCheckDeviceName(context.control.device.name);
                    this.TurnAxisInputEvent.Invoke(context.ReadValue<Vector2>() *
                                                   (this.currentDevice == Device.Gamepad
                                                       ? GameSettings.Instance.GetGamepadTurnSpeed()
                                                       : 1));
                };
            playerInput.Player.Look.canceled +=
                context => this.TurnAxisInputEvent.Invoke(context.ReadValue<Vector2>());

            playerInput.Player.Arrows.performed +=
                context => this.ArrowAxisInputEvent.Invoke(context.ReadValue<Vector2>());
            playerInput.Player.Arrows.canceled +=
                context => this.ArrowAxisInputEvent.Invoke(context.ReadValue<Vector2>());

            playerInput.Player.Sprint.performed += _ => this.RunInputEvent.Invoke(true);
            playerInput.Player.Sprint.canceled += _ => this.RunInputEvent.Invoke(false);

            playerInput.Player.Interact.performed += _ => this.InteractInputEvent.Invoke();
            playerInput.Player.Jump.performed += _ => this.JumpInputEvent.Invoke();
            playerInput.Player.Inventory.performed += _ => this.InventoryEvent.Invoke();
            playerInput.Player.Attack.performed += _ => this.ClickEvent.Invoke();
            playerInput.Player.Pause.performed += _ => this.EscapeEvent.Invoke();

            playerInput.Player.HotbarKey.performed += context =>
            {
                if (context.ReadValue<float>() == 0)
                    return;

                int index = int.Parse(context.control.displayName);
                this.HotbarKey.Invoke(index);
            };

            playerInput.Player.HotbarScroll.performed += context =>
            {
                if (context.ReadValue<float>() == 0.0f)
                    return;
                
                bool scrollUp = context.ReadValue<float>() > 0;
                this.HotbarScroll.Invoke(scrollUp);
            };

#if UNITY_EDITOR
            EditorApplication.playModeStateChanged += change => OnPlaymodeChanged(change, playerInput);
#endif
        }

#if UNITY_EDITOR
        private static void OnPlaymodeChanged(PlayModeStateChange change, IInputActionCollection playerInput)
        {
            if (change != PlayModeStateChange.ExitingPlayMode)
                return;

            playerInput.Disable();
            _instance = null;
        }
#endif

        private void OnInputCheckDeviceName(string deviceName)
        {
            Device set = Device.Null;
            if (deviceName.Contains("XInputControllerWindows"))
                set = Device.Gamepad;
            else if (deviceName.Contains("Mouse") || deviceName.Contains("Keyboard"))
                set = Device.KeyboardAndMouse;

            if (this.currentDevice == set)
                return;

            this.currentDevice = set;
            this.DeviceChangeEvent.Invoke(this.currentDevice);
        }
    }
}