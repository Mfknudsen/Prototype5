using NPCs.Base;
using ScriptableVariables.Objects;
using UnityEngine;
using UnityEngine.AI;

namespace NPCs.Enemies
{
    public class EnemyStateMachine : NpcStateMachine<EnemyStateMachine>
    {
        [Header("Chase Player")]
        public float chaseStateRange = 15.0f;
        public float viewAngle = 40.0f;
        
        [Header("Attack Player")]
        public float attackStateRange = 2.0f;
        public float damageAmount = 8.0f;
        public float attackCooldown = 1f; // in seconds

        [Header("Movement")] 
        public float npcRadius = 7.0f;
        public float chaseSpeed = 4.0f;
        public float wanderSpeed = 2.0f;

        [HideInInspector] public EnemyWanderState WanderState;
        [HideInInspector] public EnemyChaseState ChaseState;
        [HideInInspector] public EnemyAttackState AttackState;
        
        [HideInInspector] public NavMeshAgent agent;
        [HideInInspector] public Transform playerTransform;
        [HideInInspector] public CharacterHealth.Health playerHealth;
        [HideInInspector] public DamageType damageType;
        [HideInInspector] public CharacterHealth.Health enemyHealth;
        [HideInInspector] public float potionDamage;
        [HideInInspector] public DamageType potionDamageType;
        
        private EnemyDeathState _deathState;
        private EnemyGetAttackedState _getAttackedState;
        
        public float DistanceToTarget => Vector3.Distance(transform.position, playerTransform.position);

        public void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            
            WanderState = new EnemyWanderState(this);
            ChaseState = new EnemyChaseState(this);
            AttackState = new EnemyAttackState(this);
            _deathState = new EnemyDeathState(this);
            _getAttackedState = new EnemyGetAttackedState(this);
        }

        private void Start()
        {
            playerHealth = playerTransform.gameObject.GetComponent<CharacterHealth.Health>();
            SwitchState(WanderState);
        }

        public bool SeesPlayer() {
            if (!playerTransform) return false;

            Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, directionToPlayer);

            if (angle < viewAngle / 2f)
                if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, chaseStateRange))
                    if (hit.transform == playerTransform)
                        return true;

            return false;
        }

        public void OnPotionAttack(float damage, DamageType type)
        {
            potionDamage = damage;
            potionDamageType = type;
            SwitchState(_getAttackedState);
        }
        
        public void OnDeath() => SwitchState(_deathState);
    }
}
