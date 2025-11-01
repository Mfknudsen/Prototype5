using NPCs.Base;

namespace NPCs.Villagers
{
    public class VillagerSeekState : NpcState<VillagerStateMachine>
    {
        public VillagerSeekState(VillagerStateMachine fsm) : base(fsm) {}

        public override void Enter()
        {
            fsm.agent.isStopped = false;
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
