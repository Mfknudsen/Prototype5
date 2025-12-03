using NPCs.Base;

namespace NPCs.Enemies
{
    public class EnemyGetAttackedState : NpcState<EnemyStateMachine>
    {
        private const string GetAttackedAnimation = "Goblin_attacked";

        public EnemyGetAttackedState(EnemyStateMachine fsm) : base(fsm) {}
        
        public override void Enter()
        {
            fsm.enemyMovementAudio.StopMovementAudio();
            fsm.agent.isStopped = true;
            fsm.animator.CrossFade(GetAttackedAnimation, 0.1f);
        }

        public void GetAttacked()
        {
            fsm.enemyHealth.ApplyDamageType(fsm.potionDamage, fsm.potionDamageType);
        }

        public void SwitchPreviousState()
        {
            if (fsm.enemyHealth.GetIsDead()) return;
            fsm.SwitchState(fsm.previousState);
        }
    }
}
