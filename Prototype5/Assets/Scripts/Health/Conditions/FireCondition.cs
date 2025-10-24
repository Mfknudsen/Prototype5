using UnityEngine;

namespace Health.Conditions
{
    public sealed class FireCondition : MonoBehaviour
    {
        private float damage;
        
        private float currentTime, duration;

        private int tickCount;

        private void Update()
        {
            this.currentTime += Time.deltaTime;

            if (this.currentTime < this.tickCount + 1)
                return;
            
            //Damage

            if (this.currentTime >= this.duration)
                Destroy(this);
        }

        public void ResetTimer()
        {
            this.tickCount = 0;
            this.currentTime = 0;
        }
    }
}