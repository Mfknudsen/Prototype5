namespace NPCs
{
    public class NpcIdleState: NpcState<VillagerStateMachine>
    {
        private const float wanderExtraDistance = 2.0f;
        public NpcIdleState(VillagerStateMachine fsm) : base(fsm) {}

        public override void Enter()
        {
            fsm.agent.isStopped = true;
        }

        public override void UpdateLogic()
        {
            if (fsm.DistanceToTarget >= fsm.seekStateRange + wanderExtraDistance)
                fsm.SwitchState(fsm.wanderState);
        }
    }
}
