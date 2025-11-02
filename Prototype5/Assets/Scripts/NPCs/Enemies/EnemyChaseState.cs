using NPCs.Base;

namespace NPCs.Enemies
{
    public class EnemyChaseState : NpcState<EnemyStateMachine>
    {
        private const string ChaseAnimation = "Goblin_run";
        private const float TransitionTime = 0.1f;
        private const float AnimationSpeed = 1.7f; 

        public EnemyChaseState(EnemyStateMachine fsm) : base(fsm) {}

        public override void Enter()
        {
            fsm.agent.isStopped = false;
            fsm.agent.speed = fsm.chaseSpeed;

            fsm.animator.speed = AnimationSpeed;
            fsm.animator.CrossFade(ChaseAnimation, TransitionTime);
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
