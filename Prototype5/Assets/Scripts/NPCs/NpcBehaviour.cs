using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace NPCs
{
    public class NpcBehaviour : MonoBehaviour
    {
        [Header("Seek Target")]
        [SerializeField] private float maxDistanceToTarget = 20.0f;
        [SerializeField] private float minDistanceToTarget = 4.0f;
        [SerializeField] private Transform targetTransform;
        
        [Header("Prefabs")]
        [SerializeField] private GameObject[] npcPrefabs;
        [SerializeField] private Vector3 spawnPrefabOffset = new Vector3(0.0f, 0.8f, 0.0f);

        [Header("Random Movement")]
        [SerializeField] private bool useRandomWalk = true;
        [SerializeField] private float npcRadius = 15.0f;
        
        [Header("Fixed Movement")]
        [SerializeField] private Vector3[] pathPoints;
        [SerializeField] private Terrain terrain;
        
        private NavMeshAgent _agent;
        private bool _hasArrived = false;
        private int _currentIndex = 0;
        private bool _walksForward = true;
        
        void Start()
        {
            SpawnRandomPrefab();
            _agent = GetComponent<NavMeshAgent>();
            SampleTerrainHeights();
        }

        void Update()
        {
            HandleMovement();
        }

        void HandleMovement()
        {
            float distanceToTarget = Vector3.Distance(transform.position, targetTransform.position);

            if (distanceToTarget >= maxDistanceToTarget)
            {
                _hasArrived = false;
                
                if (useRandomWalk) Wander();
                else WalkPath();
            }
            else if (!_hasArrived && distanceToTarget > minDistanceToTarget) 
                SeekTarget();
            else
            {
                _agent.isStopped = true;
                _hasArrived = true;
            }
        }
    
        void SeekTarget()
        {
            _agent.isStopped = false;
            _agent.SetDestination(targetTransform.position);
        }

        void Wander()
        {
            _agent.isStopped = false;
        
            if (!_agent.pathPending && _agent.remainingDistance < 0.5f)
            {
                Vector3 randomSpherePoint = transform.position + Random.insideUnitSphere * npcRadius;
        
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomSpherePoint, out hit, npcRadius, NavMesh.AllAreas))
                    _agent.SetDestination(hit.position);   
            }
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

        void SampleTerrainHeights()
        {
            if (useRandomWalk || pathPoints.Length <= 1) return;

            for (int i = 0; i < pathPoints.Length; i++)
            {
                float height = terrain.SampleHeight(pathPoints[i]);
                pathPoints[i] = new Vector3(pathPoints[i].x, height, pathPoints[i].z);
            }
        }

        void WalkPath()
        {
            if (pathPoints.Length <= 1) return;
            
            _agent.isStopped = false;
            if (!_agent.pathPending && _agent.remainingDistance < 0.5f)
            {
                if (_walksForward)
                {
                    _currentIndex++;
                    if (_currentIndex == pathPoints.Length)
                    {
                        _currentIndex = pathPoints.Length - 2;
                        _walksForward = false;
                    }
                }
                else
                {
                    _currentIndex--;
                    if (_currentIndex < 0)
                    {
                        _currentIndex = 1;
                        _walksForward = true;
                    }
                }
                
                _agent.SetDestination(pathPoints[_currentIndex]);
            }
        }
    }
}
