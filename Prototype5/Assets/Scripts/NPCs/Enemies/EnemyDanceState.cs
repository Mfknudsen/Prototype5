using System.Collections;
using DG.Tweening;
using NPCs.Base;
using UnityEngine;

namespace NPCs.Enemies
{
    public class EnemyDanceState : NpcState<EnemyStateMachine>
    {
        private const string DanceAnimation = "Goblin_run";
        private const float TransitionTime = 0.2f;
        private const float AnimationSpeed = 0.7f;
        private bool _isDancing = false;
        
        public EnemyDanceState(EnemyStateMachine fsm) : base(fsm) {}

        public override void Enter()
        {
            fsm.agent.isStopped = true;
            
            fsm.animator.speed = AnimationSpeed;
            fsm.animator.CrossFade(DanceAnimation, TransitionTime);
        }

        public override void UpdateLogic()
        {
            Dance(fsm.jumpHeight, fsm.jumpCount, fsm.danceDuration);
        }

        private void Dance(float jumpHeight, int jumpCount, float danceDuration)
        {
            if (_isDancing) return;
            _isDancing = true;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(fsm.transform
                .DOLocalJump(fsm.transform.localPosition, jumpHeight, jumpCount, danceDuration));
            sequence.Join(fsm.transform
                .DORotate(new Vector3(0, 360, 0), danceDuration, RotateMode.LocalAxisAdd));
            sequence.OnComplete(() => {
                fsm.SwitchState(fsm.WanderState);
            });
        }
    }
}
