using ScriptableVariables.Objects;
using UnityEngine;

namespace Health.Conditions
{
    public sealed class FireCondition : MonoBehaviour
    {
        private CharacterHealth.Health health;

        private DamageType damageType;

        private float damage;

        private float currentTime, duration;

        private int tickCount;

        private bool inFire;

        public void Start()
        {
            this.tickCount = 0;
            this.currentTime = 0;

            this.health = this.GetComponent<CharacterHealth.Health>();
        }

        private void Update()
        {
            this.currentTime += Time.deltaTime;

            if (this.currentTime < this.tickCount + 1)
                return;

            this.tickCount++;

            this.health.ApplyDamageType(this.damage, this.damageType);

            if (this.inFire)
            {
                this.currentTime = 0;
                return;
            }

            if (this.currentTime >= this.duration)
                Destroy(this);
        }

        public void InFire(bool set)
        {
            this.inFire = set;
        }

        public void SetDamageType(DamageType set)
        {
            this.damageType = set;
        }
    }
}