using System.Collections;
using System.Collections.Generic;
using NPCs.Enemies;
using UnityEngine;

namespace Plants
{
    public class Thorn : MonoBehaviour
    {
        private float _timeBetweenHits;
        [SerializeField] private float damage;

        private List<Hitting> hittings = new List<Hitting>();

        public void SetTimeBetweenHits(float time) => this._timeBetweenHits = time;

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponentInParent<EnemyStateMachine>() is not { } enemyStateMachine)
                return;

            foreach (Hitting hitting in this.hittings)
            {
                if (hitting.hitObject == other.transform.parent.gameObject)
                    return;
            }

            this.hittings.Add(new Hitting
            {
                hitObject = other.transform.parent.gameObject,
                applyDamage = this.StartCoroutine(this.hit(other.transform.parent.gameObject))
            });
            enemyStateMachine.OnThornHit(this._timeBetweenHits);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponentInParent<EnemyStateMachine>() is not { } enemyStateMachine) 
                return;

            enemyStateMachine.ExitThorns();

            for (int index = this.hittings.Count - 1; index >= 0; index--)
            {
                Hitting hitting = this.hittings[index];

                if (hitting.hitObject != other.transform.parent.gameObject) continue;

                if (hitting.applyDamage != null)
                    this.StopCoroutine(hitting.applyDamage);
                this.hittings.RemoveAt(index);
            }
        }

        private IEnumerator hit(GameObject gameObject)
        {
            CharacterHealth.Health health = gameObject.GetComponent<CharacterHealth.Health>();
            while (gameObject && health)
            {
                health.ApplyDamageType(this.damage, null);

                yield return new WaitForSeconds(this._timeBetweenHits);
            }
        }

        private struct Hitting
        {
            public GameObject hitObject;
            public Coroutine applyDamage;
        }
    }
}