using UnityEngine;

namespace Plants
{
    public class ThornSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject thornsPrefab;
        [SerializeField] private float growthDuration = 1f;
        [SerializeField] private Vector3 spawnPositionOffset = Vector3.zero;
        [SerializeField] private float timeBetweenHits = 3f;

        public void SpawnThorns(Vector3 position)
        {
            GameObject thorns = Instantiate(thornsPrefab, position + spawnPositionOffset, Quaternion.identity);
            thorns.GetComponent<Thorn>().SetTimeBetweenHits(timeBetweenHits);
        }
    }
}
