using NPCs.Base;

namespace NPCs
{
    public class EnemyAttackState : NpcState<EnemyStateMachine>
    {
        private const float attackExtraDistance = 2.0f;
        public EnemyAttackState(EnemyStateMachine fsm) : base(fsm) {}

        public override void Enter()
        {
            fsm.agent.isStopped = true;
        }

        public override void UpdateLogic()
        {
            // TODO: add fov
            if (fsm.DistanceToTarget >= fsm.attackStateRange + attackExtraDistance)
                fsm.SwitchState(fsm.ChaseState);
        }
    }
}
