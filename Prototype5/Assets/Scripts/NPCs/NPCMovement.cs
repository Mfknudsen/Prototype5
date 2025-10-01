using UnityEngine;
using UnityEngine.AI;

namespace NPCs
{
    public class NpcMovement : MonoBehaviour
    {
        [SerializeField] private float maxDistanceToTarget = 20.0f;
        [SerializeField] private float minDistanceToTarget = 4.0f;
        [SerializeField] private float npcRadius = 15.0f;
        [SerializeField] private float speed = 2.5f;
        [SerializeField] private Transform targetTransform;
    
        private NavMeshAgent _agent;
        private bool _hasArrived = false;
    
        void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _agent.speed = speed;
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
    }
}
