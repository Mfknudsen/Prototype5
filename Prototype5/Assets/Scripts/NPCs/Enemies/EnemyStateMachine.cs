using NPCs.Base;
using UnityEngine;
using UnityEngine.AI;

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

        [HideInInspector] public EnemyWanderState WanderState;
        [HideInInspector] public EnemyChaseState ChaseState;
        [HideInInspector] public EnemyAttackState AttackState;
        
        [HideInInspector] public NavMeshAgent agent;
        
        public float DistanceToTarget => Vector3.Distance(transform.position, playerTransform.position);

        private void Awake()
        {
            WanderState = new EnemyWanderState(this);
            ChaseState = new EnemyChaseState(this);
            AttackState = new EnemyAttackState(this);
        }

        private void Start()
        {
            SwitchState(WanderState);
        }
    }
}
