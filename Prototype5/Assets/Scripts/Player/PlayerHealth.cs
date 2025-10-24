using ScriptableVariables.Objects;
using UnityEngine;

namespace Player
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] private float playerMaxHealth = 100.0f;
        public GameEvent deathEvent;
        
        private float _playerHealth;

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
            
            if (_playerHealth == 0.0f)
                deathEvent.InvokeGameEvents();
        }

        public void SetResistanceType(float amount, DamageInfo damageInfo)
        {
            damageInfo.multiplier = Mathf.Clamp(amount, -2.0f, 2.0f);
        }
    }
}
