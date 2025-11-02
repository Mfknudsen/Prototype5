using NPCs.Base;
using UnityEngine;

namespace NPCs.Villagers
{
    public class VillagerSeekState : NpcState<VillagerStateMachine>
    {
        private const string SeekAnimation = "Villager_Walk";
        private const float TransitionTime = 0.2f;
        private const float AnimationSpeed = 0.7f;
        
        public VillagerSeekState(VillagerStateMachine fsm) : base(fsm) {}

        public override void Enter()
        {
            fsm.agent.isStopped = false;
            
            if (fsm.animator.GetCurrentAnimatorStateInfo(0).IsName(SeekAnimation)) return;
            
            fsm.animator.speed = AnimationSpeed;
            fsm.animator.CrossFade(SeekAnimation, TransitionTime);
        }

        public override void UpdateLogic()
        {
            if (fsm.DistanceToTarget < fsm.idleStateRange)
                fsm.SwitchState(fsm.IdleState);
            else if (fsm.DistanceToTarget >= fsm.seekStateRange)
                fsm.SwitchState(fsm.WanderState);
        }

        public override void UpdatePhysics()
        {
            SeekTarget();
        }

        private void SeekTarget()
        {
            fsm.agent.SetDestination(fsm.targetTransform.position);
        }
    }
}
