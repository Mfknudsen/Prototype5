using DayNightCycle;
using DG.Tweening;
using UnityEngine;

namespace Prefabs.Audio
{
    public class PlayerAudio : MonoBehaviour
    {
        [SerializeField] private float fadeDuration = 2f;
        
        private AudioSource _audioSource;
        private float _targetVolume;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _targetVolume = _audioSource?.volume ?? 0.2f;
        }

        private void OnEnable()
        {
            DayNight.AddListener(CheckAudioVolume);
        }

        private void OnDisable()
        {
            DayNight.RemoveListener(CheckAudioVolume);
        }


        private void CheckAudioVolume(DayNightTime dayNightTime)
        {
            switch (dayNightTime)
            {
                case DayNightTime.Evening:
                    _audioSource.DOFade(0f, fadeDuration);
                    return;
                case DayNightTime.Morning:
                    _audioSource.DOFade(_targetVolume, fadeDuration);
                    break;
            }
        }
    }
}
