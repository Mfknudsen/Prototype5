using System;
using DayNightCycle;
using ScriptableVariables.Objects;
using UnityEngine;
using UnityEngine.Events;

namespace NPCs.Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Enemy Information")]
        public bool useNightMobs = true;
        [SerializeField] private GameObject nightMobPrefab;
        [SerializeField] private GameObject dayMobPrefab;
        [SerializeField] private Vector3[] spawnPositions;
        [SerializeField] private EnemySpawnerReference enemySpawnerReference;
        
        [Header("Attack Player")]
        public Transform playerTransform;
        public DamageType damageType;
        
        private static bool _enemiesSpawned;
        
        private void Awake()
        {
            if (enemySpawnerReference)
                enemySpawnerReference.value = this;
            else
                Debug.Log("Enemy Spawner Reference is null");
        }

        private void OnEnable()
        {
            DayNight.AddListener(CheckSpawnEnemies);
        }

        private void OnDisable()
        {
            DayNight.RemoveListener(CheckSpawnEnemies);
        }

        public void SpawnMobs()
        {
            if (spawnPositions.Length == 0) return;

            foreach (var position in spawnPositions)
                InstantiateMob(position);
        }

        public void DespawnMobs()
        {
            for (int i = 0; i < transform.childCount; i++)
                transform.GetChild(i).gameObject.GetComponent<EnemyStateMachine>().OnDeath();
        }

        private void InstantiateMob(Vector3 position)
        {
            GameObject mobPrefab = useNightMobs ? nightMobPrefab : dayMobPrefab;
            GameObject mob = Instantiate(mobPrefab, position, Quaternion.identity, transform);
            
            EnemyStateMachine enemyStateMachine = mob.GetComponent<EnemyStateMachine>();
            enemyStateMachine.playerTransform = playerTransform;
            enemyStateMachine.damageType = damageType;
            
            CharacterHealth.Health enemyHealth = mob.GetComponent<CharacterHealth.Health>();
            enemyHealth.LocalDeathAction += enemyStateMachine.OnDeath;
        }

        private void OnDestroy()
        {            
            if (enemySpawnerReference) 
                enemySpawnerReference.value = null;
        }
        
        private void CheckSpawnEnemies(DayNightTime dayNightTime)
        {
            var spawnTime = useNightMobs ? DayNightTime.Night : DayNightTime.Morning;
            var despawnTime = useNightMobs ? DayNightTime.Morning : DayNightTime.Night;

            if (dayNightTime == spawnTime && !_enemiesSpawned)
            {
                SpawnMobs();
                _enemiesSpawned = true;
            }
            else if (dayNightTime == despawnTime && _enemiesSpawned)
            {
                DespawnMobs();
                _enemiesSpawned = false;
            }
        }
    }
}
