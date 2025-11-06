using ScriptableVariables.Objects;
using UnityEngine;

namespace NPCs.Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Enemy Information")] 
        [SerializeField] private GameObject nightMobPrefab;
        [SerializeField] private GameObject dayMobPrefab;
        [SerializeField] private Vector3[] spawnPositions;
        [SerializeField] private EnemySpawnerReference enemySpawnerReference;
        
        [Header("Attack Player")]
        public Transform playerTransform;
        public DamageType damageType;

        public bool useNightMobs = true;

        private void Awake()
        {
            if (enemySpawnerReference)
                enemySpawnerReference.value = this;
            else
                Debug.Log("Enemy Spawner Reference is null");
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
    }
}
