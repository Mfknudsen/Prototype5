using System.Collections;
using System.Collections.Generic;
using Inventory;
using Potions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameWorld
{
    public sealed class PotionSpawner : MonoBehaviour
    {
        [SerializeField] private SpawnerType spawnerType;

        [SerializeField] private PotionValue potion;

        [SerializeField] [Min(1)] private int spawnCount = 1;

        [SerializeField] [Min(0)] [Tooltip("In Seconds")]
        private float timeBetweenSpawns;

        [SerializeField] [Tooltip("Spawn randomly between these points. If no points then spawn at the spawner.")]
        private List<Transform> spawnPoints;

        private int spawnedCount;

        private float currentTime;

        private IEnumerator Start()
        {
            if (this.spawnerType != SpawnerType.Once)
                yield break;

            for (int i = 0; i < this.spawnCount; i++)
            {
                if (this.spawnPoints.Count == 0)
                {
                    Instantiate(this.potion.GetPrefab(), this.transform.position, this.transform.rotation);
                }
                else
                {
                    int index = Random.Range(0, this.spawnPoints.Count);
                    Transform t = this.spawnPoints[index];
                    Instantiate(this.potion.GetPrefab(), t.position, t.rotation);
                }

                yield return new WaitForSeconds(this.timeBetweenSpawns);
            }

            Destroy(this.gameObject);
        }

        private void Update()
        {
            if (this.spawnerType == SpawnerType.Once)
                return;

            if (this.spawnedCount >= this.spawnCount)
                return;

            this.currentTime += Time.deltaTime / 60.0f;

            if (this.currentTime < this.timeBetweenSpawns)
                return;

            this.currentTime -= this.timeBetweenSpawns;

            if (this.spawnPoints.Count == 0)
            {
                InventoryItem item =
                    Instantiate(this.potion.GetPrefab(), this.transform.position, this.transform.rotation)
                        .GetComponent<InventoryItem>();
                item.AddEventListener(this.OnTriggerAction);
            }
            else
            {
                int index = Random.Range(0, this.spawnPoints.Count);
                Transform t = this.spawnPoints[index];
                InventoryItem item =
                    Instantiate(this.potion.GetPrefab(), t.position, t.rotation).GetComponent<InventoryItem>();
                item.AddEventListener(this.OnTriggerAction);
            }

            this.spawnedCount++;
        }

        private void OnTriggerAction(InventoryItem item)
        {
            this.spawnedCount--;
            item.RemoveEventListener(this.OnTriggerAction);
        }
    }
}