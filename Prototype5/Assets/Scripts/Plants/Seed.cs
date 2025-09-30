using UnityEngine;

namespace Plants
{
    public sealed class Seed : MonoBehaviour
    {
        [SerializeField] private GameObject PlantPrefab;

        public GameObject GetPrefab()
        {
            return this.PlantPrefab;
        }
    }
}