using UnityEngine;
using UnityEngine.AI;

namespace NPCs
{
    public class NpcWanderState : NpcState
    {
        private int _currentIndex = 0;
        private bool _walksForward = true;
        
        public NpcWanderState(NpcBehaviour npcBehaviour) : base(npcBehaviour) {}

        public override void Enter()
        {
            npcBehaviour.agent.isStopped = false;
        }

        public override void UpdateLogic()
        {
            if (npcBehaviour.DistanceToTarget < npcBehaviour.minDistanceToTarget)
            {
                npcBehaviour.SwitchState(npcBehaviour.seekState);
            }
        }

        public override void UpdatePhysics()
        {
            if (npcBehaviour.useRandomWalk) Wander();
            else WalkPath();
        }
        
        void Wander()
        {
            NavMeshAgent agent = npcBehaviour.agent;
            agent.isStopped = false;
        
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                Vector3 randomSpherePoint = npcBehaviour.transform.position + Random.insideUnitSphere * npcBehaviour.npcRadius;
        
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomSpherePoint, out hit, npcBehaviour.npcRadius, NavMesh.AllAreas))
                    agent.SetDestination(hit.position);   
            }
        }
        
        void WalkPath()
        {
            if (npcBehaviour.pathPoints.Length <= 1) return;
            
            NavMeshAgent agent = npcBehaviour.agent;
            agent.isStopped = false;
            
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                if (_walksForward)
                {
                    _currentIndex++;
                    if (_currentIndex == npcBehaviour.pathPoints.Length)
                    {
                        _currentIndex = npcBehaviour.pathPoints.Length - 2;
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
                
                agent.SetDestination(npcBehaviour.pathPoints[_currentIndex]);
            }
        }
    }
}
