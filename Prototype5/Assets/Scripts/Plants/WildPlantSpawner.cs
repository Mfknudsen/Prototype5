using DayNightCycle;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Plants
{
    public sealed class WildPlantSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject plantPrefab;

        [SerializeField] [Min(1)] private int maxSpawnCount;

        [SerializeField] [Min(0.1f)] private float spawnRadius;

        [SerializeField] [Min(0)] [Tooltip("In minutes")]
        private float timeBetweenSpawn;

        [Header("Day/Night Spawn Timer")] [SerializeField]
        private float minTimeSpawn;

        [SerializeField] private float maxTimeSpawn;

        [SerializeField] private bool invert;

        private float time;

        private int spawnedCount;

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(this.transform.position, this.spawnRadius);
        }

        private void Start()
        {
            if (this.plantPrefab == null)
            {
                Debug.LogError("No Plant Prefab to Spawn");
            }

            this.time = this.timeBetweenSpawn;
        }

        private void Update()
        {
            if (!this.CheckDayNightTime())
                return;

            if (this.spawnedCount >= this.maxSpawnCount)
                return;

            this.time += Time.deltaTime / 60.0f;

            if (this.time < this.timeBetweenSpawn)
                return;

            this.time -= this.timeBetweenSpawn;

            Vector3 randomDirection = Random.insideUnitSphere * this.spawnRadius;

            randomDirection += this.transform.position;
            NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, this.spawnRadius, 1);
            Vector3 finalPosition = hit.position;

            Plant plant =
                Instantiate(this.plantPrefab, finalPosition, quaternion.identity).GetComponent<Plant>();
            plant.AddEventListener(this.OnTriggerAction);
            plant.transform.localScale = Vector3.zero;
            plant.transform.DOScale(Vector3.one, 0.5f);
            this.spawnedCount++;
        }

        private bool CheckDayNightTime()
        {
            if (Mathf.Approximately(this.minTimeSpawn, this.maxTimeSpawn))
                return true;

            int currentHour = DayNight.GetCurrentHour();

            if (!this.invert)
                return currentHour >= this.minTimeSpawn && currentHour <= this.maxTimeSpawn;

            return currentHour >= this.maxTimeSpawn && currentHour <= this.minTimeSpawn;
        }

        private void OnTriggerAction(Plant plant)
        {
            this.spawnedCount--;
            plant.RemoveEventListener(this.OnTriggerAction);
        }
    }
}