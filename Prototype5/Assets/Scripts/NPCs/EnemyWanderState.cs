using UnityEngine;
using UnityEngine.AI;

namespace NPCs
{
    public class EnemyWanderState : NpcState<EnemyStateMachine>
    {
        public EnemyWanderState(EnemyStateMachine fsm) : base(fsm) {}

        public override void Enter()
        {
            fsm.agent.isStopped = false;
            fsm.agent.speed = fsm.wanderSpeed;
        }

        public override void UpdateLogic()
        {
            // TODO: add fov
            if (fsm.DistanceToTarget < fsm.chaseStateRange)
                fsm.SwitchState(fsm.chaseState);
        }

        public override void UpdatePhysics()
        {
            Wander();
        }
        
        void Wander()
        {
            NavMeshAgent agent = fsm.agent;
            agent.isStopped = false;
        
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
