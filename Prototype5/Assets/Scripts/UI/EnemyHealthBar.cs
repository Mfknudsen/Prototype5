using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class EnemyHealthBar : MonoBehaviour
    {
        [SerializeField] private float speed = 1;
        [SerializeField] private Gradient colorGradient;
        [SerializeField] private UnityEvent<float> onProgress;
        [SerializeField] private UnityEvent onComplete;
        
        private Image _progressImage;
        private Coroutine _progressAnimation;
        private Vector3 _offsetFromEnemy;
        private Transform _enemyTransform;
        private Camera _playerCamera;

        public void SetEnemyTransform(Transform enemyTransform)
        {
            _enemyTransform = enemyTransform;
        }

        public void SetPlayerCamera(Camera playerCamera)
        {
            _playerCamera = playerCamera;
        }

        private void Update()
        {
            LookAtPlayer();
            FollowEnemy();
        }
        
        private void Awake()
        {
            _offsetFromEnemy = transform.localPosition;
            _progressImage = GetComponent<Image>();
            
            if (_progressImage == null || _progressImage.type != Image.Type.Filled)
                Debug.LogError("Object doesn't have valid image.");
        }

        public void SetProgress(float progressAmount)
        {
            if (progressAmount is < 0 or > 1)
            {
                progressAmount = Mathf.Clamp01(progressAmount);
            }

            if (!Mathf.Approximately(progressAmount, _progressImage.fillAmount))
            {
                if (_progressAnimation != null)
                    StopCoroutine(_progressAnimation);
                _progressAnimation = StartCoroutine(AnimateProgress(progressAmount));
            }

            IEnumerator AnimateProgress(float amount)
            {
                float time = 0f;
                float initialAmount = _progressImage.fillAmount;

                while (time < 1f)
                {
                    _progressImage.fillAmount = Mathf.Lerp(initialAmount, amount, time);
                    time += Time.deltaTime * speed;

                    _progressImage.color = colorGradient.Evaluate(1 - _progressImage.fillAmount);
                    onProgress?.Invoke(_progressImage.fillAmount);

                    yield return null;
                }

                _progressImage.fillAmount = progressAmount;
                _progressImage.color = colorGradient.Evaluate(1 - _progressImage.fillAmount);
                onProgress?.Invoke(_progressImage.fillAmount);
                onComplete?.Invoke();
            }
        }

        private void FollowEnemy()
        {
            if (_enemyTransform)
                transform.position = _enemyTransform.position + _offsetFromEnemy;
        }

        private void LookAtPlayer()
        {
            if (_playerCamera)
                transform.LookAt(_playerCamera.transform, Vector3.up);
        } 
        
        public void DestroyEnemyHealthBar() => Destroy(gameObject);

    }
}
