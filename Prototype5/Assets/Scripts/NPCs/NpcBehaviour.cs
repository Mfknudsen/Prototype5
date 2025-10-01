using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace NPCs
{
    public class NpcBehaviour : MonoBehaviour
    {
        [SerializeField] private float maxDistanceToTarget = 20.0f;
        [SerializeField] private float minDistanceToTarget = 4.0f;
        [SerializeField] private float npcRadius = 15.0f;
        [SerializeField] private Transform targetTransform;
        [SerializeField] private GameObject[] npcPrefabs;
        [SerializeField] private Vector3 spawnPrefabOffset = new Vector3(0.0f, 0.8f, 0.0f);
        
        private NavMeshAgent _agent;
        private bool _hasArrived = false;
    
        void Start()
        {
            SpawnRandomPrefab();
            _agent = GetComponent<NavMeshAgent>();
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
                Wander();
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
    }
}
