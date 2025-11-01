using NPCs.Base;
using UnityEngine;
using UnityEngine.AI;

namespace NPCs
{
    public class VillagerWanderState : NpcState<VillagerStateMachine>
    {
        private int _currentIndex = 0;
        private bool _walksForward = true;
        
        public VillagerWanderState(VillagerStateMachine fsm) : base(fsm) {}

        public override void Enter()
        {
            fsm.agent.isStopped = false;
        }

        public override void UpdateLogic()
        {
            if (fsm.DistanceToTarget < fsm.seekStateRange)
            {
                fsm.SwitchState(fsm.SeekState);
            }
        }

        public override void UpdatePhysics()
        {
            if (fsm.useRandomWalk) Wander();
            else WalkPath();
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
        
        void WalkPath()
        {
            if (fsm.pathPoints.Length <= 1) return;
            
            NavMeshAgent agent = fsm.agent;
            agent.isStopped = false;
            
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                if (_walksForward)
                {
                    _currentIndex++;
                    if (_currentIndex == fsm.pathPoints.Length)
                    {
                        _currentIndex = fsm.pathPoints.Length - 2;
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
                
                agent.SetDestination(fsm.pathPoints[_currentIndex]);
            }
        }
    }
}
