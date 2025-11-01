using NPCs.Base;
using UnityEngine;
using System.Collections;

namespace NPCs.Enemies
{
    public class EnemyAttackState : NpcState<EnemyStateMachine>
    {
        private bool canAttack = true;
        
        public EnemyAttackState(EnemyStateMachine fsm) : base(fsm) {}

        public override void Enter()
        {
            fsm.agent.isStopped = true;
        }

        public override void UpdateLogic()
        {
            if (canAttack)
                fsm.StartCoroutine(AttackPlayerCoroutine());
            else
                fsm.SwitchState(fsm.WanderState);
        }

        private void AttackPlayer()
        {
            fsm.playerHealth.ApplyDamageType(fsm.damageAmount, fsm.damageType);
            canAttack = false;
        }

        private IEnumerator AttackPlayerCoroutine()
        {
            AttackPlayer();
            yield return new WaitForSeconds(fsm.attackCooldown);
            canAttack = true;
        }
    }
}
