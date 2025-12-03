using UnityEngine;

namespace NPCs.Enemies
{
    public class EnemyMovementAudio : MonoBehaviour
    {
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void StartMovementAudio()
        {
            if (!_audioSource.isPlaying)
                _audioSource.Play();
        }

        public void StopMovementAudio()
        {
            if (_audioSource.isPlaying)
                _audioSource.Play();
        }
    }
}
