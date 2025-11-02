using NPCs.Base;
using UnityEngine;

namespace NPCs.Villagers
{
    public class VillagerIdleState: NpcState<VillagerStateMachine>
    {
        private const string IdleAnimation = "Villager_Idle";
        private const float TransitionTime = 0.1f;
        private const float AnimationSpeed = 1.0f;
        private const float WanderExtraDistance = 2.0f;
        
        public VillagerIdleState(VillagerStateMachine fsm) : base(fsm) {}

        public override void Enter()
        {
            fsm.agent.isStopped = true;
            
            fsm.animator.speed = AnimationSpeed;
            fsm.animator.CrossFade(IdleAnimation, TransitionTime);
        }

        public override void UpdateLogic()
        {
            if (fsm.DistanceToTarget >= fsm.seekStateRange + WanderExtraDistance)
                fsm.SwitchState(fsm.WanderState);
        }
    }
}
