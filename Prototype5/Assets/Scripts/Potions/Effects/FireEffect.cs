namespace Potions.Effects
{
    public sealed class FireEffect : IncludeSelfEffectBase
    {
        protected override EffectTargetTag[] exclude()
        {
            return new EffectTargetTag[]
            {
                EffectTargetTag.All
            };
        }

        protected override void Effect(PotionObject potionObject, PotionEffectTarget target)
        {
        }

        public override void TriggerSelf(PotionObject potionObject)
        {
            throw new System.NotImplementedException();
        }
    }
}