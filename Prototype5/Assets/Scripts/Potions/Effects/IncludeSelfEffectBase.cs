namespace Potions.Effects
{
    public abstract class IncludeSelfEffectBase : PotionEffectBase
    {
        public abstract void TriggerSelf(PotionObject potionObject);
    }
}