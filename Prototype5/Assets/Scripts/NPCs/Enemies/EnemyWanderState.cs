using NPCs.Base;
using UnityEngine;
using UnityEngine.AI;

namespace NPCs.Enemies
{
    public class EnemyWanderState : NpcState<EnemyStateMachine>
    {
        private const string WanderAnimation = "Goblin_run";
        private const float TransitionTime = 0.2f;
        private const float AnimationSpeed = 0.7f;
            
        public EnemyWanderState(EnemyStateMachine fsm) : base(fsm) {}
        
        public override void Enter()
        {
            fsm.agent.isStopped = false;
            fsm.agent.speed = fsm.wanderSpeed;
            
            fsm.animator.speed = AnimationSpeed;
            fsm.animator.CrossFade(WanderAnimation, TransitionTime);
        }

        public override void UpdateLogic()
        {
            if (fsm.DistanceToTarget <= fsm.chaseStateRange && fsm.SeesPlayer())
                fsm.SwitchState(fsm.ChaseState);
        }

        public override void UpdatePhysics()
        {
            Wander();
        }
        
        void Wander()
        {
            NavMeshAgent agent = fsm.agent;
        
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                Vector3 randomSpherePoint = fsm.transform.position + Random.insideUnitSphere * fsm.npcRadius;
        
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomSpherePoint, out hit, fsm.npcRadius, NavMesh.AllAreas))
                    agent.SetDestination(hit.position);   
            }
        }
    }
}
