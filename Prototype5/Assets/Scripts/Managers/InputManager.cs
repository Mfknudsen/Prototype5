using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    public sealed class InputManager
    {
        public static InputManager Instance => _instance ??= new InputManager();

        private static InputManager _instance;

        public readonly UnityEvent<Vector2>
            MoveAxisInputEvent = new UnityEvent<Vector2>(),
            TurnAxisInputEvent = new UnityEvent<Vector2>();

        public readonly UnityEvent
            InteractInputEvent = new UnityEvent(),
            JumpInputEvent = new UnityEvent(),
            InventoryEvent = new UnityEvent();

        public readonly UnityEvent<bool>
            RunInputEvent = new UnityEvent<bool>();

        private InputManager()
        {
            InputSystem_Actions playerInput = new InputSystem_Actions();

            playerInput.Player.Enable();

            playerInput.Player.Move.performed +=
                context => this.MoveAxisInputEvent.Invoke(context.ReadValue<Vector2>());
            playerInput.Player.Move.canceled +=
                context => this.MoveAxisInputEvent.Invoke(context.ReadValue<Vector2>());

            playerInput.Player.Look.performed +=
                context => this.TurnAxisInputEvent.Invoke(context.ReadValue<Vector2>());
            playerInput.Player.Look.canceled +=
                context => this.TurnAxisInputEvent.Invoke(context.ReadValue<Vector2>());

            playerInput.Player.Sprint.performed += _ => this.RunInputEvent.Invoke(true);
            playerInput.Player.Sprint.canceled += _ => this.RunInputEvent.Invoke(false);

            playerInput.Player.Interact.performed += _ => this.InteractInputEvent.Invoke();
            playerInput.Player.Jump.performed += _ => this.JumpInputEvent.Invoke();
            playerInput.Player.Interact.performed += _ => this.InventoryEvent.Invoke();
        }
    }
}