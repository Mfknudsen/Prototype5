using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace NPCs
{
    public class EnemyStateMachine : NpcStateMachine<EnemyStateMachine>
    {
        [Header("Seek Target")]
        public float chaseStateRange = 10.0f;
        public float attackStateRange = 2.0f;
        public Transform playerTransform;

        [Header("Movement")] 
        public float npcRadius = 7.0f;
        public float chaseSpeed = 4.0f;
        public float wanderSpeed = 2.0f;

        [HideInInspector] public EnemyWanderState wanderState;
        [HideInInspector] public EnemyChaseState chaseState;
        [HideInInspector] public EnemyAttackState attackState;
        
        [HideInInspector] public NavMeshAgent agent;
        
        public float DistanceToTarget => Vector3.Distance(transform.position, playerTransform.position);

        private void Awake()
        {
            wanderState = new EnemyWanderState(this);
            chaseState = new EnemyChaseState(this);
            attackState = new EnemyAttackState(this);
        }

        private void Start()
        {
            SwitchState(wanderState);
        }
    }
}
