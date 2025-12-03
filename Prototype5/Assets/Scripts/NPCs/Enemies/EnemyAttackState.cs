using System;
using NPCs.Base;
using UnityEngine;
using System.Linq;
using Object = UnityEngine.Object;

namespace NPCs.Enemies
{
    [Serializable]
    public class EnemyAttackState : NpcState<EnemyStateMachine>
    {
        private const string AttackAnimation = "Goblin_attack";

        private bool _canAttack = true;

        public EnemyAttackState(EnemyStateMachine fsm) : base(fsm) {}

        public override void Enter()
        {
            this.fsm.agent.isStopped = true;
            CalculateAnimationSpeed();
            this.fsm.animator.Play(AttackAnimation);
        }

        public override void UpdateLogic()
        {
            if (this.fsm.DistanceToTarget >= this.fsm.attackStateRange)
                this.fsm.SwitchState(this.fsm.WanderState);
            else if (_canAttack)
                this.fsm.animator.Play(AttackAnimation);
        }

        public void AttackPlayer()
        {
            this.fsm.playerHealth.ApplyDamageType(this.fsm.damageAmount, this.fsm.damageType);
            PlayAttackSound();
            this._canAttack = false;
        }
        
        public void ResetCanAttack()
        {
            this._canAttack = true;
        }

        private void PlayAttackSound()
        {
            if (fsm.audioSource && fsm.onAttackSound is {} sound && sound)
            {
                GameObject soundObject = new GameObject();
                fsm.audioSource.PlayOneShot(sound);
                Object.Destroy(soundObject, (float)(sound.length + 0.01));
            }
        }

        private void CalculateAnimationSpeed()
        {
            AnimationClip clip =
                fsm.animator
                    .runtimeAnimatorController
                    .animationClips
                    .FirstOrDefault(
                        clip => clip.name == AttackAnimation
                    );
            
            if (clip)
            {
                float defaultDuration = clip.averageDuration;
                this.fsm.animator.speed = defaultDuration / fsm.attackCooldown;
            }
        }
    }
}