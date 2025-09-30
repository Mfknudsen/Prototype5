using Interactions;
using ScriptableVariables.Objects;
using UnityEngine;

namespace Plants
{
    public sealed class PlantingArea : MonoBehaviour, IInteractable
    {
        [SerializeField] private TransformVariable handTransform;

        public void OnTrigger()
        {
            if (this.handTransform.Value.childCount == 0)
                return;

            Seed seed = null;
            foreach (Transform t in this.handTransform.Value)
            {
                if (t.TryGetComponent(out seed))
                    break;
            }

            if (seed == null)
                return;

            Transform seedTransform = Instantiate(seed.GetPrefab()).transform;
            seedTransform.parent = this.transform;
            seedTransform.position = this.transform.position;
            seedTransform.rotation = this.transform.rotation;

            Destroy(seed.gameObject);
        }
    }
}