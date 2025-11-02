using System;
using System.Collections.Generic;
using ScriptableVariables.Objects;
using UnityEngine;

namespace CharacterHealth
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float maxHealth = 100.0f;
        public GameEvent deathEvent;

        private float currentHealth;

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
            if (damageType != null)
            {
                foreach (Resistance resistance in this.resistances)
                {
                    if (resistance.damageType == damageType)
                        amount *= resistance.multiplier;
                }
            }

            this.currentHealth = Mathf.Max(0.0f, this.currentHealth - amount);

            if (this.currentHealth == 0.0f)
                this.deathEvent?.InvokeGameEvents();
        }

        [Serializable]
        private struct Resistance
        {
            [SerializeField] public DamageType damageType;
            [SerializeField] public float multiplier;
        }
    }
}