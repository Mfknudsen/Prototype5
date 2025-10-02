using Interactions;
using Potions;
using UnityEngine;

namespace Plants
{
    public class Plant : MonoBehaviour, IInteractable
    {
        [SerializeField] [Tooltip("Time in minutes")]
        private float growthTime;

        [SerializeField] private IngredientValue toSpawnIngredientValue;

        [SerializeField] private int minSpawnCount, maxSpawnCount;

        [SerializeField] private GameObject stageOne, stageTwo, stageThree;

        [SerializeField] [Tooltip("Time in minutes to switch to next visual growth stage")]
        private float timeToStageTwo, timeToStageThree;

        private float currentTime;

        private void Start()
        {
            this.currentTime = 0;
            this.stageTwo.SetActive(false);
            this.stageThree.SetActive(false);
        }

        private void Update()
        {
            if (this.currentTime / 60.0f > this.growthTime)
                return;

            this.currentTime += Time.deltaTime;

            if (this.stageOne.activeSelf)
            {
                if (this.currentTime / 60.0f > this.timeToStageTwo)
                {
                    this.stageOne.SetActive(false);
                    this.stageTwo.SetActive(true);
                }
            }

            if (!this.stageTwo.activeSelf) return;

            if (!(this.currentTime / 60.0f > this.timeToStageThree)) return;

            this.stageTwo.SetActive(false);
            this.stageThree.SetActive(true);
        }

        public void OnTrigger()
        {
            if (this.currentTime / 60.0f < this.growthTime)
                return;

            const float offset = 0.3f;

            for (int i = 0; i < Random.Range(this.minSpawnCount, this.maxSpawnCount); i++)
            {
                Vector3 spawnPoint = this.transform.position + new Vector3(
                    Random.Range(-offset, offset),
                    0,
                    Random.Range(-offset, offset));

                Transform t = Instantiate(this.toSpawnIngredientValue.GetPrefab()).transform;
                t.position = spawnPoint;
            }

            this.transform.parent.GetComponent<SphereCollider>().enabled = true;
            Destroy(this.gameObject);
        }

        public bool IsActive()
        {
            return this.enabled;
        }
    }
}