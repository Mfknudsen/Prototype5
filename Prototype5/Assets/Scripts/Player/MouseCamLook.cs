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

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            if (this.playerStateVariable.Value != PlayerStateEnum.Free)
                return;

            float mouseX = Input.GetAxis("Mouse X") * this.mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * this.mouseSensitivity * Time.deltaTime;

            this.xRotation -= mouseY;
            this.xRotation = Mathf.Clamp(this.xRotation, -90f, 90f);

            this.transform.localRotation = Quaternion.Euler(this.xRotation, 0f, 0f);

            this.playerBody.Rotate(Vector3.up * mouseX);
        }
    }
}