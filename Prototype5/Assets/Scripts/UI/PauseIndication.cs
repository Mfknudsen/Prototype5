using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI
{
    public class PauseIndication : MonoBehaviour
    {
        [SerializeField] private float fadeTime = 2f;
        [SerializeField] private float delayBeforeFade = 2f;
        private TextMeshProUGUI pauseIndicationText;

        private void Start()
        {
            FadeText();
        }

        private void FadeText()
        {
            pauseIndicationText = GetComponent<TextMeshProUGUI>();
            pauseIndicationText
                .DOFade(0, fadeTime)
                .SetDelay(delayBeforeFade);
        }
    }
}
