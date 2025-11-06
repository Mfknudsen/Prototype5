using NPCs.Base;
using UnityEngine;

namespace NPCs.Enemies
{
    public class EnemyDeathState : NpcState<EnemyStateMachine>
    {
        private const string DeathAnimation = "Goblin_death";
        private const float TimeBeforeDespawn = 2f;
        
        public EnemyDeathState(EnemyStateMachine fsm) : base(fsm) {}

        public override void Enter()
        {
            fsm.agent.isStopped = true;
            fsm.animator.CrossFade(DeathAnimation, 0.1f);
        }

        public override void UpdateLogic()
        {
            DespawnEnemy();
        }

        private void DespawnEnemy()
        {
            float animationLength = fsm.animator.GetCurrentAnimatorClipInfo(0).Length;
            Object.Destroy(fsm.gameObject, animationLength + TimeBeforeDespawn);
        }
    }
}
