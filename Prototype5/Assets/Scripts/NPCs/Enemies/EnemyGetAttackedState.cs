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
            GetAttacked();
        }
        
        private void GetAttacked()
        {
            if (!_isAttacked)
            {
                _isAttacked = true;
                fsm.StartCoroutine(ApplyDamageCoroutine(SwitchStateDelay));                
            }
        }
        
        private IEnumerator ApplyDamageCoroutine(float seconds)
        {
            fsm.enemyHealth.ApplyDamageType(fsm.potionDamage, fsm.potionDamageType);
            yield return new WaitForSeconds(seconds);
            fsm.SwitchState(fsm.previousState);
        }
    }
}
