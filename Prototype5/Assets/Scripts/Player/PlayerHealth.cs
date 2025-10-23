using ScriptableVariables.Objects;
using UnityEngine;

namespace Player
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] private float playerMaxHealth = 100.0f;
        
        private float _playerHealth;
        private bool _isDead;

        private void Awake()
        {
            _playerHealth = playerMaxHealth;
        }

        public void ApplyHeal(float amount)
        {
            _playerHealth = Mathf.Min(playerMaxHealth, _playerHealth + amount);
        }

        public void ApplyDamageType(float damageAmount, DamageInfo damageInfo)
        {
            float amount = damageAmount - damageInfo.multiplier * damageAmount;
            _playerHealth = Mathf.Max(0.0f, _playerHealth - amount);
        }

        public void SetResistanceType(float amount, DamageInfo damageInfo)
        {
            damageInfo.multiplier = Mathf.Clamp(amount, -2.0f, 2.0f);
        }

        public void OnDeath()
        {
            _isDead = true;
        }
    }
}
