using NPCs.Base;
using UnityEngine;
using System.Collections;

namespace NPCs.Enemies
{
    public class EnemyAttackState : NpcState<EnemyStateMachine>
    {
        private const string AttackAnimation = "Goblin_attack";
        private const float AnimationSpeed = 1.0f;
        
        private bool _canAttack = true;
        
        public EnemyAttackState(EnemyStateMachine fsm) : base(fsm) {}

        public override void Enter()
        {
            fsm.agent.isStopped = true;
            
            fsm.animator.speed = AnimationSpeed;
            fsm.animator.Play(AttackAnimation);
        }

        public override void UpdateLogic()
        {
            if (fsm.DistanceToTarget >= fsm.attackStateRange)
                fsm.SwitchState(fsm.WanderState);
            else if (_canAttack)
                fsm.StartCoroutine(AttackPlayerCoroutine());
        }

        private void AttackPlayer()
        {
            fsm.playerHealth.ApplyDamageType(fsm.damageAmount, fsm.damageType);
            _canAttack = false;
        }

        private IEnumerator AttackPlayerCoroutine()
        {
            AttackPlayer();
            yield return new WaitForSeconds(fsm.attackCooldown);
            _canAttack = true;
        }
    }
}
