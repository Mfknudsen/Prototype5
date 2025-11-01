using NPCs.Base;

namespace NPCs.Enemies
{
    public class EnemyChaseState : NpcState<EnemyStateMachine>
    {
        public EnemyChaseState(EnemyStateMachine fsm) : base(fsm) {}

        public override void Enter()
        {
            fsm.agent.isStopped = false;
            fsm.agent.speed = fsm.chaseSpeed;
        }

        public override void UpdateLogic()
        {
            if (fsm.DistanceToTarget > fsm.chaseStateRange || !fsm.SeesPlayer())
                fsm.SwitchState(fsm.WanderState);
            else if (fsm.DistanceToTarget < fsm.attackStateRange && fsm.SeesPlayer())
                fsm.SwitchState(fsm.AttackState);
        }

        public override void UpdatePhysics()
        {
            ChasePlayer();
        }

        private void ChasePlayer()
        {
            fsm.agent.SetDestination(fsm.playerTransform.position);
        }
    }
}
