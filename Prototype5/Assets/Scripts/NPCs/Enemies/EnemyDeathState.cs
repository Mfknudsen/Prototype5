using NPCs.Base;
using UnityEngine;

namespace NPCs.Enemies
{
    public class EnemyDeathState : NpcState<EnemyStateMachine>
    {
        private const string DeathAnimation = "Goblin_death";
        
        public EnemyDeathState(EnemyStateMachine fsm) : base(fsm) {}

        public override void Enter()
        {
            fsm.enemyMovementAudio.StopMovementAudio();
            fsm.agent.isStopped = true;
            fsm.animator.CrossFade(DeathAnimation, 0.1f);
        }

        public void DespawnEnemy()
        {
            Object.Destroy(fsm.gameObject);
        }
        
        public void PlayDeathSound()
        {
            if (fsm.onDeathSound is {} sound && sound)
            {
                GameObject soundObject = new GameObject();
                AudioSource audioSource = soundObject.AddComponent<AudioSource>();
                audioSource.volume = fsm.deathSoundVolume;
                audioSource.PlayOneShot(sound);
                Object.Destroy(soundObject, (float)(sound.length + 0.01));
            }
        }
    }
}
