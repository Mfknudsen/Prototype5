using ScriptableVariables.Objects;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
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

        private void Start()
        {
            this.playerTransformVariable.Value = this.transform;
            this.cameraTransformVariable.Value = this.cameraTransform;
            this.handTransformVariable.Value = this.handTransform;
        }

        private void Update()
        {
            if (this.controller.isGrounded && this.velocity.y < 0)
                this.velocity.y = -2f;

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            this.speedBoost = Input.GetButton("Fire3") ? this.sprintSpeed : 1f;


            Vector3 move = this.transform.right * x + this.transform.forward * z;

            this.controller.Move(move * ((this.baseSpeed + this.speedBoost) * Time.deltaTime));

            if (Input.GetButtonDown("Jump") && this.controller.isGrounded)
            {
                this.velocity.y = Mathf.Sqrt(this.jumpHeight * -2f * this.gravity);
            }

            this.velocity.y += this.gravity * Time.deltaTime;

            this.controller.Move(this.velocity * Time.deltaTime);
        }
    }
}