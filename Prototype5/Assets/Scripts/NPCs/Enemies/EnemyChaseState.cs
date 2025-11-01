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
            // TODO: check if the player is out of the fov
            if (fsm.DistanceToTarget >= fsm.chaseStateRange)
                fsm.SwitchState(fsm.WanderState);
            else if (fsm.DistanceToTarget < fsm.attackStateRange)
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
