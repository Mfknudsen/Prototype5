using System;
using System.Collections.Generic;
using ScriptableVariables.Objects;
using UnityEngine;

namespace CharacterHealth
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float maxHealth = 100.0f;
        public GameEvent globalDeathEvent;
        public Action LocalDeathAction;

        public float _currentHealth;

        [SerializeField] private List<Resistance> resistances = new List<Resistance>();

        private void Start()
        {
            _currentHealth = maxHealth;
        }

        public void ApplyHeal(float amount)
        {
            _currentHealth = Mathf.Min(maxHealth, _currentHealth + amount);
        }

        public void ApplyDamageType(float damageAmount, DamageType damageType)
        {
            float amount = damageAmount;
            if (damageType)
            {
                foreach (Resistance resistance in resistances)
                {
                    if (resistance.damageType == damageType)
                        amount *= resistance.multiplier;
                }
            }

            _currentHealth = Mathf.Max(0.0f, _currentHealth - amount);

            if (_currentHealth == 0.0f)
                OnDeath();
        }

        private void OnDeath()
        {
            LocalDeathAction?.Invoke();
            globalDeathEvent?.InvokeGameEvents();
        }

        [Serializable]
        private struct Resistance
        {
            [SerializeField] public DamageType damageType;
            [SerializeField] public float multiplier;
        }
    }
}