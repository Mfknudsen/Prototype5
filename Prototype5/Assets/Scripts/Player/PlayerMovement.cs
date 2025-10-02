using Managers;
using ScriptableVariables.Enums;
using ScriptableVariables.Objects;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private PlayerStateVariable playerStateVariable;

        [SerializeField] private Transform cameraTransform, handTransform;

        [SerializeField] private TransformVariable cameraTransformVariable,
            handTransformVariable,
            playerTransformVariable;

        public CharacterController controller;
        public float baseSpeed = 12f;
        public float gravity = -9.81f;
        public float jumpHeight = 3f;
        public float sprintSpeed = 5f;

        private float speedBoost = 1f;
        private Vector3 velocity;

        private Vector2 moveDirection;

        private bool run;

        private void Start()
        {
            this.playerTransformVariable.Value = this.transform;
            this.cameraTransformVariable.Value = this.cameraTransform;
            this.handTransformVariable.Value = this.handTransform;
        }

        private void OnEnable()
        {
            InputManager.Instance.MoveAxisInputEvent.AddListener(this.UpdateMoveDirectionInput);
            InputManager.Instance.JumpInputEvent.AddListener(this.OnJumpInput);
            InputManager.Instance.RunInputEvent.AddListener(this.OnRunInput);
        }

        private void OnDisable()
        {
            InputManager.Instance.MoveAxisInputEvent.RemoveListener(this.UpdateMoveDirectionInput);
            InputManager.Instance.JumpInputEvent.RemoveListener(this.OnJumpInput);
            InputManager.Instance.RunInputEvent.RemoveListener(this.OnRunInput);
        }

        private void Update()
        {
            if (this.playerStateVariable.Value != PlayerStateEnum.Free)
                return;

            if (this.controller.isGrounded && this.velocity.y < 0)
                this.velocity.y = -2f;

            this.speedBoost = this.run ? this.sprintSpeed : 1f;

            Vector3 move = this.transform.right * this.moveDirection.x + this.transform.forward * this.moveDirection.y;

            this.controller.Move(move * ((this.baseSpeed + this.speedBoost) * Time.deltaTime));

            this.velocity.y += this.gravity * Time.deltaTime;

            this.controller.Move(this.velocity * Time.deltaTime);
        }

        private void UpdateMoveDirectionInput(Vector2 input)
        {
            this.moveDirection = input;
        }

        private void OnJumpInput()
        {
            if (this.controller.isGrounded)
                this.velocity.y = Mathf.Sqrt(this.jumpHeight * -2f * this.gravity);
        }

        private void OnRunInput(bool input)
        {
            this.run = input;
        }
    }
}