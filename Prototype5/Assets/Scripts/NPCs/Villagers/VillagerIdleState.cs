using NPCs.Base;

namespace NPCs.Villagers
{
    public class VillagerIdleState: NpcState<VillagerStateMachine>
    {
        private const float wanderExtraDistance = 2.0f;
        public VillagerIdleState(VillagerStateMachine fsm) : base(fsm) {}

        public override void Enter()
        {
            fsm.agent.isStopped = true;
        }

        public override void UpdateLogic()
        {
            if (fsm.DistanceToTarget >= fsm.seekStateRange + wanderExtraDistance)
                fsm.SwitchState(fsm.WanderState);
        }
    }
}
