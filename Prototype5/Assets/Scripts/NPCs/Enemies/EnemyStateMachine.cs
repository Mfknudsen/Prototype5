using NPCs.Base;
using ScriptableVariables.Objects;
using UnityEngine;
using UnityEngine.AI;

namespace NPCs.Enemies
{
    public class EnemyStateMachine : NpcStateMachine<EnemyStateMachine>
    {
        [Header("Chase Player")]
        public float chaseStateRange = 10.0f;
        public Transform playerTransform;
        public float viewAngle = 40.0f;
        
        [Header("Attack Player")]
        public float attackStateRange = 2.0f;
        public float damageAmount = 8.0f;
        public DamageType damageType;
        public float attackCooldown = 1f; // in seconds

        [Header("Movement")] 
        public float npcRadius = 7.0f;
        public float chaseSpeed = 4.0f;
        public float wanderSpeed = 2.0f;

        [HideInInspector] public EnemyWanderState WanderState;
        [HideInInspector] public EnemyChaseState ChaseState;
        [HideInInspector] public EnemyAttackState AttackState;
        
        [HideInInspector] public NavMeshAgent agent;
        [HideInInspector] public CharacterHealth.Health playerHealth;
        
        public float DistanceToTarget => Vector3.Distance(transform.position, playerTransform.position);

        private void Awake()
        {
            WanderState = new EnemyWanderState(this);
            ChaseState = new EnemyChaseState(this);
            AttackState = new EnemyAttackState(this);

            playerHealth = playerTransform.gameObject.GetComponent<CharacterHealth.Health>();
        }

        private void Start()
        {
            SwitchState(WanderState);
        }

        public bool SeesPlayer() {
            if (playerTransform == null) return false;

            Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, directionToPlayer);

            if (angle < viewAngle / 2f)
                if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, chaseStateRange))
                    if (hit.transform == playerTransform)
                        return true;

            return false;
        }
    }
}
