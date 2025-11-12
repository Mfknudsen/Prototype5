using System.Collections;
using NPCs.Base;
using UnityEngine;

namespace NPCs.Enemies
{
    public class EnemyGetAttackedState : NpcState<EnemyStateMachine>
    {
        private const string GetAttackedAnimation = "Goblin_attacked";
        private const float SwitchStateDelay = 0.7f;
        
        private bool _isAttacked = false;

        public EnemyGetAttackedState(EnemyStateMachine fsm) : base(fsm) {}
        
        public override void Enter()
        {
            fsm.agent.isStopped = true;
            fsm.animator.CrossFade(GetAttackedAnimation, 0.1f);
        }

        public override void UpdateLogic()
        {
            ApplyDamage();
            fsm.StartCoroutine(AttackSequence(SwitchStateDelay));
        }
        
        private void ApplyDamage()
        {
            if (_isAttacked) return;
            
            fsm.enemyHealth.ApplyDamageType(fsm.potionDamage, fsm.potionDamageType);
            _isAttacked = true;
        }
        
        private IEnumerator AttackSequence(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            fsm.SwitchState(fsm.WanderState);
        }
    }
}
