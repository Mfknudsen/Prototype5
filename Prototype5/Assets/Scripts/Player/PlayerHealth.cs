using ScriptableVariables.Objects;
using UnityEngine;

namespace Player
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] private float playerMaxHealth = 100.0f;
        public GameEvent deathEvent;
        
        [SerializeField] private DamageType testDamageType;
        private float _playerHealth;

        private void Update()
        {
            TestGameOver();
        }

        private void Awake()
        {
            _playerHealth = playerMaxHealth;
        }

        public void ApplyHeal(float amount)
        {
            _playerHealth = Mathf.Min(playerMaxHealth, _playerHealth + amount);
        }

        public void ApplyDamageType(float damageAmount, DamageType damageType)
        {
            float amount = damageAmount - damageType.multiplier * damageAmount;
            _playerHealth = Mathf.Max(0.0f, _playerHealth - amount);
            
            if (_playerHealth == 0.0f)
                deathEvent.InvokeGameEvents();
        }

        public void SetResistanceType(float amount, DamageType damageType)
        {
            damageType.multiplier = Mathf.Clamp(amount, -1.0f, 1.0f);
        }

        private void TestGameOver()
        {
            if (Input.GetKeyDown(KeyCode.T))
                ApplyDamageType(30, testDamageType);
        }
    }
}
