using NPCs.Base;

namespace NPCs.Enemies
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
            if (fsm.DistanceToTarget >= fsm.attackStateRange + attackExtraDistance && fsm.SeesPlayer())
                fsm.SwitchState(fsm.ChaseState);
        }
    }
}
