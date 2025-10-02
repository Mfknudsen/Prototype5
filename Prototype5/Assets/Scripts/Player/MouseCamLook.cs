using System;
using Managers;
using ScriptableVariables.Enums;
using UnityEngine;

namespace Player
{
    public class MouseCamLook : MonoBehaviour
    {
        [SerializeField] private PlayerStateVariable playerStateVariable;

        public float mouseSensitivity = 100f;
        public Transform playerBody;
        private float xRotation;

        private Vector2 mouseInput;

        private void OnEnable()
        {
            InputManager.Instance.TurnAxisInputEvent.AddListener(this.OnMouseInput);
        }

        private void OnDisable()
        {
            InputManager.Instance.TurnAxisInputEvent.RemoveListener(this.OnMouseInput);
        }

        private void Update()
        {
            if (this.playerStateVariable.Value != PlayerStateEnum.Free)
                return;

            float mouseX = this.mouseInput.x * this.mouseSensitivity * Time.deltaTime;
            float mouseY = this.mouseInput.y * this.mouseSensitivity * Time.deltaTime;

            this.xRotation -= mouseY;
            this.xRotation = Mathf.Clamp(this.xRotation, -90f, 90f);

            this.transform.localRotation = Quaternion.Euler(this.xRotation, 0f, 0f);

            this.playerBody.Rotate(Vector3.up * mouseX);
        }

        private void OnMouseInput(Vector2 input)
        {
            this.mouseInput = input;
        }
    }
}