using DG.Tweening;
using Interactions;
using ScriptableVariables.Objects;
using UnityEngine;

namespace UI.Book
{
    public sealed class BookInteract : MonoBehaviour, IInteractable
    {
        [SerializeField] private UIBook uiBook;
        [SerializeField] private Transform playerReadPosition;
        [SerializeField] private TransformVariable playerTransform, cameraTransform;

        public void OnTrigger()
        {
            this.uiBook.Effect(BookTurn.Open);
            this.playerTransform.Value.DOMove(this.playerReadPosition.position, 0.25f);
            this.playerTransform.Value.DORotateQuaternion(this.playerReadPosition.rotation, 0.25f);
            this.cameraTransform.Value.DOLocalRotate(new Vector3(30, 0, 0), 0.25f);
        }

        public bool IsActive()
        {
            return this.gameObject.activeSelf;
        }

        public Vector3? Hover()
        {
            return this.transform.position;
        }
        
        public string GetInteractName()
        {
            return this.gameObject.name;
        }
    }
}