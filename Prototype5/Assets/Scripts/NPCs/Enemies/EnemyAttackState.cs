using NPCs.Base;
using UnityEngine;
using System.Collections;

namespace NPCs.Enemies
{
    public class EnemyAttackState : NpcState<EnemyStateMachine>
    {
        private const float attackExtraDistance = 2.0f;
        private bool canAttack = true;
        
        public EnemyAttackState(EnemyStateMachine fsm) : base(fsm) {}

        public override void Enter()
        {
            fsm.agent.isStopped = true;
        }

        public override void UpdateLogic()
        {
            if (fsm.DistanceToTarget >= fsm.attackStateRange + attackExtraDistance && fsm.SeesPlayer())
                fsm.SwitchState(fsm.ChaseState);
            else if (canAttack)
                fsm.StartCoroutine(AttackPlayerCoroutine());
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
