using System;
using DayNightCycle;
using ScriptableVariables.Objects;
using UI;
using UnityEditor;
using UnityEngine;

namespace NPCs.Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Testing")] [HideInInspector] public bool inTestingScene;
        [HideInInspector] public Vector3[] testSpawnPositions;

        [Header("Enemy Information")] public bool useNightMobs = true;
        [SerializeField] private GameObject nightMobPrefab;
        [SerializeField] private GameObject dayMobPrefab;
        [SerializeField] private Canvas enemyHealthBarCanvas;
        [SerializeField] private Vector3[] spawnPositions;

        [Header("Attack Player")] public Transform playerTransform;
        public DamageType damageType;

        private static bool _enemiesSpawned;
        private Camera _playerCamera;

        private void Awake()
        {
            _enemiesSpawned = false;
            _playerCamera = playerTransform.GetComponentInChildren<Camera>();
            if (_playerCamera == null)
                Debug.Log("Player camera not found.");

            SpawnTestEnemies();
        }

        private void Update()
        {
            
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

            EnemyHealthBar enemyHealthBar = mob.GetComponentInChildren<EnemyHealthBar>();
            enemyHealthBar.SetEnemyTransform(mob.transform);
            enemyHealthBar.SetPlayerCamera(_playerCamera);
            enemyHealthBar.transform.SetParent(enemyHealthBarCanvas.transform);

            CharacterHealth.Health enemyHealth = mob.GetComponent<CharacterHealth.Health>();
            enemyStateMachine.enemyHealth = enemyHealth;
            enemyHealth.localDeathAction.AddListener(enemyStateMachine.OnDeath);
            enemyHealth.localDeathAction.AddListener(enemyHealthBar.DestroyEnemyHealthBar);
            enemyHealth.localHealthChangeAction.AddListener(enemyHealthBar.SetProgress);
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
            if (dayNightTime == spawnTime && !_enemiesSpawned)
            {
                SpawnMobs();
                _enemiesSpawned = true;
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