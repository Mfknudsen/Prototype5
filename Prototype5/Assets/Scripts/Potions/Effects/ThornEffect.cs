using System;
using Plants;
using Potions.Effects.PersistentEffects;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Potions.Effects
{
    [Serializable]
    public sealed class ThornEffect : IncludeSelfEffectBase
    {
        [SerializeField] private GameObject persistentThornGameObject;
        [SerializeField] private float duration;

        protected override void Effect(PotionObject potionObject, PotionEffectTarget target)
        {
            //Effect will be handled by spawned persistent effect object
        }

        public override void TriggerSelf(PotionObject potionObject)
        {
            PersistentThornEffect persistentThornEffect =
                Object.Instantiate(this.persistentThornGameObject).GetComponent<PersistentThornEffect>();
            
            persistentThornEffect.transform.position = potionObject.transform.position;
            
            if (persistentThornEffect.gameObject.GetComponent<ThornSpawner>() is { } thornSpawner)
                thornSpawner.SpawnThorns(potionObject.transform.position);

            persistentThornEffect.Trigger(this.effectRadius, this.duration);
        }
    }
}