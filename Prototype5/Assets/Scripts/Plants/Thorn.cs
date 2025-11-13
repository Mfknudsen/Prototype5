using NPCs.Enemies;
using UnityEngine;

namespace Plants
{
    public class Thorn : MonoBehaviour
    {
        private float _timeBetweenHits;

        public void SetTimeBetweenHits(float time) => _timeBetweenHits = time;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponentInParent<EnemyStateMachine>() is { } enemyStateMachine)
                enemyStateMachine.OnThornHit(_timeBetweenHits);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponentInParent<EnemyStateMachine>() is { } enemyStateMachine)
                enemyStateMachine.ExitThorns();
        }
    }
}
