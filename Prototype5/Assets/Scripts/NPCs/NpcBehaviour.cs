using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace NPCs
{
    public class NpcBehaviour : NpcStateMachine
    {
        [Header("Seek Target")]
        public float maxDistanceToTarget = 20.0f;
        public float minDistanceToTarget = 4.0f;
        public Transform targetTransform;
        
        [Header("Prefabs")]
        [SerializeField] private GameObject[] npcPrefabs;
        [SerializeField] private Vector3 spawnPrefabOffset = new Vector3(0.0f, 0.8f, 0.0f);

        [Header("Movement")]
        [HideInInspector] public bool useRandomWalk = true;
        [HideInInspector] public float npcRadius = 15.0f;
        [HideInInspector] public Vector3[] pathPoints;
        
        [HideInInspector] public NpcWanderState wanderState;
        [HideInInspector] public NpcState seekState;
        [HideInInspector] public NpcState idleState;
        
        public NavMeshAgent agent;

        public float DistanceToTarget => Vector3.Distance(transform.position, targetTransform.position);
        
        private void Awake()
        {
            SpawnRandomPrefab();
            agent = GetComponent<NavMeshAgent>();
            
            wanderState = new NpcWanderState(this);
            seekState = new NpcSeekState(this);
            idleState = new NpcIdleState(this);
        }

        void Start()
        {
            SwitchState(wanderState);
        }
        
        void SpawnRandomPrefab()
        {
            if (npcPrefabs.Length >= 1)
            {
                int random = Random.Range(0, npcPrefabs.Length);
                Instantiate(npcPrefabs[random], transform.position + spawnPrefabOffset,
                    Quaternion.identity, transform);                
            }
            else
            {
                Debug.Log("NPC prefab list is empty!");
            }
        }
        
    }

    [CustomEditor(typeof(NpcBehaviour))]
    public class NpcBehaviourEditor : Editor
    {
        private SerializedProperty _useRandomWalkProperty;
        private SerializedProperty _pathPointsProperty;
        private SerializedProperty _npcRadiusProperty;
        
        private void OnEnable()
        {
            _useRandomWalkProperty = serializedObject.FindProperty("useRandomWalk");
            _pathPointsProperty = serializedObject.FindProperty("pathPoints");
            _npcRadiusProperty = serializedObject.FindProperty("npcRadius");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawDefaultInspector();

            EditorGUILayout.PropertyField(_useRandomWalkProperty);

            if (!_useRandomWalkProperty.boolValue)
            {
                EditorGUILayout.PropertyField(_pathPointsProperty, new GUIContent("Path points"), true);
            }
            else
            {
                EditorGUILayout.PropertyField(_npcRadiusProperty);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
