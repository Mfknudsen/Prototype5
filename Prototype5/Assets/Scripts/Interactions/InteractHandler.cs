using System.Net;
using Managers;
using ScriptableVariables.Enums;
using ScriptableVariables.Objects;
using TMPro;
using UnityEngine;
using Utils;

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
        
        // Used to display the name on mouse hover 
        [SerializeField] private TextMeshProUGUI interactLabel;
        
        private Camera cam;

        // The currently highlighted object
        private Highlightable lastHighlighted;
        private IInteractable lastDebugInteractable = null;


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

            RaycastHit[] results = new RaycastHit[16];
            int size = Physics.SphereCastNonAlloc(ray, .25f, results, this.maxDistance, this.layerMask);

            IInteractable closest = null;

            if (size != 0)
            {
                for (int i = 0; i < size; i++)
                {
                    RaycastHit raycastHit = results[i];

                    if (Vector3.Distance(raycastHit.point, this.playerTransform.Position) > this.maxDistance)
                        continue;

                    if (!raycastHit.collider.TryGetComponent(out IInteractable interactable))
                        continue;

                    if (!interactable.IsActive())
                        continue;

                    closest = interactable;
                }
            }
            
            UpdateHighlight(closest);

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

            if (this.current != null && this.current.Hover() == null)
            {
                if (this.uiInteractButtonTransform.gameObject.activeSelf)
                    this.uiInteractButtonTransform.gameObject.SetActive(false);
                return;
            }

            if (this.current.Hover() is { } hoverPosition)
            {
                if (!this.uiInteractButtonTransform.gameObject.activeSelf)
                    this.uiInteractButtonTransform.gameObject.SetActive(true);

                Vector3 position = this.cam.WorldToScreenPoint(hoverPosition);
                this.uiInteractButtonTransform.position = position;
            }
        }
        
        private void UpdateHighlight(IInteractable closest)
        {
            // Unhighlight the previous highlighted object
            if (lastHighlighted != null)
            {
                lastHighlighted.Unhighlight();
                lastHighlighted = null;
            }

            // If no current interactable, stop here
            if (closest == null)
                return;

            // Find Highlightable on the same object
            if (closest is MonoBehaviour mb)
            {
                Highlightable h = mb.GetComponent<Highlightable>();
                if (h != null)
                {
                    h.Highlight();
                    lastHighlighted = h;
                }
            }
            
            // Only log when object CHANGES
            if (closest != lastDebugInteractable)
            {
                Debug.Log($"[InteractHandler] Closest interactable: {closest} on object {((MonoBehaviour)closest).gameObject.name}");

                lastDebugInteractable = closest;
            }
            
            DisplayHighlightedName(closest);

        }

        private void DisplayHighlightedName(IInteractable closest)
        {
            if (closest == null)
            {
                interactLabel.text = "E";
                return;
            }

            string name;

            if (closest.GetInteractName().Contains("Book"))
            {   
                name = NameUtils.CleanBookName(closest.GetInteractName());
            }
            else
            {
                name = NameUtils.CleanName(closest.GetInteractName());
            }
            
            // Set UI text
            interactLabel.text = $"Interact with: {name}";
        }

        private void OnInputTrigger()
        {
            if (this.playerStateVariable.Value != PlayerStateEnum.Free)
                return;

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
