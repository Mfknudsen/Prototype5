using System;
using NPCs.Base;
using UnityEngine;
using System.Collections;
using Object = UnityEngine.Object;

namespace NPCs.Enemies
{
    [Serializable]
    public class EnemyAttackState : NpcState<EnemyStateMachine>
    {
        private const string AttackAnimation = "Goblin_attack";
        private const float AnimationSpeed = 1.0f;

        [SerializeField] private AudioClip attackAudioClip;
        [SerializeField] private AudioSource audioSource;

        private bool _canAttack = true;

        public EnemyAttackState(EnemyStateMachine fsm) : base(fsm)
        {
        }

        public override void Enter()
        {
            this.fsm.agent.isStopped = true;

            this.fsm.animator.speed = AnimationSpeed;
            this.fsm.animator.Play(AttackAnimation);
            PlayAttackSound();
        }

        public override void UpdateLogic()
        {
            if (this.fsm.DistanceToTarget >= this.fsm.attackStateRange)
                this.fsm.SwitchState(this.fsm.WanderState);
            else if (this._canAttack) this.fsm.StartCoroutine(this.AttackPlayerCoroutine());
        }

        private void AttackPlayer()
        {
            this.fsm.playerHealth.ApplyDamageType(this.fsm.damageAmount, this.fsm.damageType);

            if (this.audioSource && this.attackAudioClip)
                this.audioSource.PlayOneShot(this.attackAudioClip);
            
            this._canAttack = false;
        }

        private IEnumerator AttackPlayerCoroutine()
        {
            this.AttackPlayer();
            yield return new WaitForSeconds(this.fsm.attackCooldown);
            this._canAttack = true;
        }

        private void PlayAttackSound()
        {
            if (fsm.onAttackSound is {} sound && sound)
            {
                GameObject soundObject = new GameObject();
                soundObject.AddComponent<AudioSource>().PlayOneShot(sound);
                Object.Destroy(soundObject, (float)(sound.length + 0.01));
            }
        }
    }
}