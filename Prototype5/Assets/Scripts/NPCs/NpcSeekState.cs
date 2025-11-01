namespace NPCs
{
    public class NpcSeekState : NpcState<VillagerStateMachine>
    {
        public NpcSeekState(VillagerStateMachine fsm) : base(fsm) {}

        public override void Enter()
        {
            fsm.agent.isStopped = false;
        }

        public override void UpdateLogic()
        {
            if (fsm.DistanceToTarget < fsm.idleStateRange)
                fsm.SwitchState(fsm.idleState);
            else if (fsm.DistanceToTarget >= fsm.seekStateRange)
                fsm.SwitchState(fsm.wanderState);
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
