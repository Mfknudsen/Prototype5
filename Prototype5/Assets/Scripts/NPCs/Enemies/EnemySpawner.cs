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
        
        [Header("Attack Player")]
        public Transform playerTransform;
        public DamageType damageType;

        private bool _useNightMobs = true;

        private void Start()
        {
            SpawnMobs();
        }

        private void SpawnMobs()
        {
            if (spawnPositions.Length == 0) return;

            foreach (var position in spawnPositions)
                InstantiateMob(position);
        }

        private void InstantiateMob(Vector3 position)
        {
            GameObject mob = _useNightMobs ? nightMobPrefab : dayMobPrefab;
                
            EnemyStateMachine enemyStateMachine = mob.GetComponent<EnemyStateMachine>();
            enemyStateMachine.playerTransform = playerTransform;
            enemyStateMachine.damageType = damageType;
                
            CharacterHealth.Health enemyHealth = 
                Instantiate(mob, position, Quaternion.identity).GetComponent<CharacterHealth.Health>();
            enemyHealth.LocalDeathAction += enemyStateMachine.OnDeath;
        }
    }
}
