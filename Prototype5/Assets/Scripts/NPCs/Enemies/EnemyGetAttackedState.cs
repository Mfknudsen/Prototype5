using NPCs.Base;

namespace NPCs.Enemies
{
    public class EnemyGetAttackedState : NpcState<EnemyStateMachine>
    {
        private const string GetAttackedAnimation = "Goblin_get_attacked";
        private const int StopFrame = 28;
        private const int TotalFrames = 70;
        private const float AnimationSpeed = 0.84f;

        public EnemyGetAttackedState(EnemyStateMachine fsm) : base(fsm) {}
        
        public override void Enter()
        {
            fsm.agent.isStopped = true;
            fsm.animator.speed = AnimationSpeed;
            fsm.animator.Play(GetAttackedAnimation, 1, 1f / TotalFrames * StopFrame);
        }

        public override void UpdateLogic()
        {
            ApplyDamage();
            fsm.SwitchState(fsm.WanderState);
        }

        public override void Exit()
        {
            fsm.animator.speed = 1;
        }

        private void ApplyDamage()
        {
            fsm.enemyHealth.ApplyDamageType(fsm.potionDamage, fsm.potionDamageType);
        }
    }
}
