using System;
using DayNightCycle;
using ScriptableVariables.Objects;
using UnityEditor;
using UnityEngine;

namespace NPCs.Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Testing")] 
        [HideInInspector] public bool inTestingScene;
        [HideInInspector] public Vector3[] testSpawnPositions;
        
        [Header("Enemy Information")]
        public bool useNightMobs = true;
        [SerializeField] private GameObject nightMobPrefab;
        [SerializeField] private GameObject dayMobPrefab;
        [SerializeField] private Vector3[] spawnPositions;
        
        [Header("Attack Player")]
        public Transform playerTransform;
        public DamageType damageType;
        
        private static bool _enemiesSpawned;

        private void Awake()
        {
            SpawnTestEnemies();
        }

        private void OnEnable()
        {
            if (inTestingScene) return;
            DayNight.AddListener(CheckSpawnEnemies);
        }

        private void OnDisable()
        {
            if (inTestingScene) return;
            DayNight.RemoveListener(CheckSpawnEnemies);
        }

        private void SpawnMobs()
        {
            if (spawnPositions.Length == 0) return;

            foreach (var position in spawnPositions)
                InstantiateMob(position);
        }

        private void DespawnMobs()
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
            enemyStateMachine.enemyHealth = enemyHealth;
            enemyHealth.LocalDeathAction += enemyStateMachine.OnDeath;
        }

        private void SpawnTestEnemies()
        {
            if (!inTestingScene) return;
            SpawnMobsAtLocations(testSpawnPositions);
        }
        
        private void SpawnMobsAtLocations(Vector3[] positions)
        {
            if (positions.Length == 0) return;

            foreach (var position in positions)
                InstantiateMob(position);
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

#if UNITY_EDITOR
    [CustomEditor(typeof(EnemySpawner))]
    public class EnemySpawnerEditor : Editor
    {
        private SerializedProperty _inTestingScene;
        private SerializedProperty _testSpawnPositions;

        private void OnEnable()
        {
            _inTestingScene = serializedObject.FindProperty("inTestingScene");
            _testSpawnPositions = serializedObject.FindProperty("testSpawnPositions");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawDefaultInspector();

            EditorGUILayout.PropertyField(_inTestingScene);

            if (_inTestingScene.boolValue)
                EditorGUILayout.PropertyField(_testSpawnPositions);

            serializedObject.ApplyModifiedProperties();
        }
    }
    
#endif
}
