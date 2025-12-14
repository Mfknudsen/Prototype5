using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI
{
    public class EnemiesIndication : MonoBehaviour
    {
        [SerializeField] private float fadeTime = 2f;
        [SerializeField] private float delayBeforeFade = 4f;
        private TextMeshProUGUI _enemiesIndicationText;

        private void Start()
        {
            _enemiesIndicationText = GetComponent<TextMeshProUGUI>();
        }

        private void FadeInText()
        {
            _enemiesIndicationText
                .DOFade(1, fadeTime);
        }

        private void FadeOutText()
        {
            _enemiesIndicationText
                .DOFade(0, fadeTime)
                .SetDelay(delayBeforeFade)
                .OnComplete(() => gameObject.SetActive(false));
        }

        public void ShowEnemiesIndication()
        {
            FadeInText();
            FadeOutText();
        }
    }
}
