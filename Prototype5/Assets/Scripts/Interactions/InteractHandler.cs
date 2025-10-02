using Managers;
using ScriptableVariables.Objects;
using UnityEngine;

namespace Interactions
{
    public sealed class InteractHandler : MonoBehaviour
    {
        [SerializeField] private LayerMask layerMask;

        [SerializeField] private TransformVariable cameraTransform, playerTransform;

        [SerializeField] private float maxDistance = 5.0f;

        private bool down;

        private void OnEnable()
        {
            InputManager.Instance.InteractInputEvent.AddListener(this.OnInputTrigger);
        }

        private void OnDisable()
        {
            InputManager.Instance.InteractInputEvent.RemoveListener(this.OnInputTrigger);
        }

        private void OnInputTrigger()
        {
            Debug.Log("Trigger");
            Debug.Log($"{this.cameraTransform.Value == null} | {this.playerTransform.Value == null}");

            if (this.cameraTransform.Value == null || this.playerTransform.Value == null)
                return;

            Ray ray = new Ray(this.cameraTransform.Position, this.cameraTransform.Forward);

            // ReSharper disable once Unity.PreferNonAllocApi
            RaycastHit[] hits =
                Physics.SphereCastAll(ray, .1f, this.maxDistance, this.layerMask);

            IInteractable closest = null;

            Debug.Log(hits.Length);
            if (hits.Length == 0)
                return;

            foreach (RaycastHit raycastHit in hits)
            {
                Debug.Log(
                    $"{raycastHit.collider.gameObject.name} : {Vector3.Distance(raycastHit.collider.transform.position, this.playerTransform.Position)}");
                if (Vector3.Distance(raycastHit.collider.transform.position, this.playerTransform.Position) >
                    this.maxDistance)
                    continue;

                if (!raycastHit.collider.gameObject.TryGetComponent(out IInteractable interactable))
                    continue;

                closest = interactable;
            }

            closest?.OnTrigger();
        }
    }
}