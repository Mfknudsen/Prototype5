using Managers;
using ScriptableVariables.Enums;
using ScriptableVariables.Objects;
using UnityEngine;

namespace Interactions
{
    public sealed class InteractHandler : MonoBehaviour
    {
        [SerializeField] private PlayerStateVariable playerStateVariable;

        [SerializeField] private RectTransform uiInteractButtonTransform;

        [SerializeField] private Canvas canvas;

        [SerializeField] private LayerMask layerMask;

        [SerializeField] private TransformVariable cameraTransform, playerTransform;

        [SerializeField] private float maxDistance = 5.0f;

        private Camera cam;

        private bool down;

        private IInteractable current;

        private void Start()
        {
            this.uiInteractButtonTransform.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            InputManager.Instance.InteractInputEvent.AddListener(this.OnInputTrigger);

            if (this.cameraTransform.Value)
            {
                this.cam = this.cameraTransform.Value.GetComponent<Camera>();
            }

            this.cameraTransform.AddListener(this.OnCameraTransformUpdate);
        }

        private void OnDisable()
        {
            InputManager.Instance.InteractInputEvent.RemoveListener(this.OnInputTrigger);
            this.cameraTransform.RemoveListener(this.OnCameraTransformUpdate);
        }

        private void Update()
        {
            if (!this.cameraTransform.Value || !this.playerTransform.Value)
                return;

            Ray ray = new Ray(this.cameraTransform.Position, this.cameraTransform.Forward);

            // ReSharper disable once Unity.PreferNonAllocApi
            RaycastHit[] hits =
                Physics.SphereCastAll(ray, .25f, this.maxDistance, this.layerMask);

            IInteractable closest = null;

            if (hits.Length == 0)
                return;

            foreach (RaycastHit raycastHit in hits)
            {
                if (Vector3.Distance(raycastHit.point, this.playerTransform.Position) >
                    this.maxDistance)
                    continue;

                if (!raycastHit.collider.gameObject.TryGetComponent(out IInteractable interactable))
                    continue;

                if (!interactable.IsActive())
                    continue;

                closest = interactable;
            }

            this.current = closest;
        }

        private void LateUpdate()
        {
            if (!this.cam)
                return;

            if (this.current == null)
            {
                if (this.uiInteractButtonTransform.gameObject.activeSelf)
                    this.uiInteractButtonTransform.gameObject.SetActive(false);
                return;
            }

            if (this.current != null && this.playerStateVariable.Value != PlayerStateEnum.Free)
            {
                this.uiInteractButtonTransform.gameObject.SetActive(false);
                return;
            }

            if (this.current != null && !this.uiInteractButtonTransform.gameObject.activeSelf)
                this.uiInteractButtonTransform.gameObject.SetActive(true);

            Vector3 position = this.cam.WorldToScreenPoint(this.current.Hover());
            this.uiInteractButtonTransform.position = position;
        }

        private void OnInputTrigger()
        {
            if (this.playerStateVariable.Value != PlayerStateEnum.Free)
                return;

            Debug.Log("Trigger");
            this.current?.OnTrigger();
        }

        private void OnCameraTransformUpdate(Transform t)
        {
            this.cam = null;
            if (!t)
                return;

            this.cam = this.cameraTransform.Value.GetComponent<Camera>();
        }
    }
}