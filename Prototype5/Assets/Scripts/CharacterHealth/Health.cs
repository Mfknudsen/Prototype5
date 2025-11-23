using System;
using System.Collections.Generic;
using ScriptableVariables.Objects;
using UnityEngine;
using UnityEngine.Events;

namespace CharacterHealth
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float maxHealth = 100.0f;
        public GameEvent globalDeathEvent;
        public UnityEvent localDeathAction = new UnityEvent();
        public UnityEvent<float> localHealthChangeAction = new UnityEvent<float>();

        [SerializeField]private float currentHealth;

        [SerializeField] private List<Resistance> resistances = new List<Resistance>();

        private void Start()
        {
            this.currentHealth = this.maxHealth;
        }

        public void ApplyHeal(float amount)
        {
            this.currentHealth = Mathf.Min(this.maxHealth, this.currentHealth + amount);
        }

        public void ApplyDamageType(float damageAmount, DamageType damageType)
        {
            float amount = damageAmount;
            if (damageType)
            {
                foreach (Resistance resistance in this.resistances)
                {
                    if (resistance.damageType == damageType)
                        amount *= resistance.multiplier;
                }
            }

            this.currentHealth = Mathf.Max(0.0f, this.currentHealth - amount);
            this.localHealthChangeAction.Invoke(this.currentHealth / this.maxHealth);

            if (this.currentHealth == 0.0f) this.OnDeath();
        }

        private void OnDeath()
        {
            this.localDeathAction?.Invoke();
            this.globalDeathEvent?.InvokeGameEvents();
        }

        [Serializable]
        private struct Resistance
        {
            [SerializeField] public DamageType damageType;
            [SerializeField] public float multiplier;
        }
    }
}