using System.Collections.Generic;

namespace ScriptableVariables
{
    public abstract class ListVariable<TGeneric> : ScriptableVariable<List<TGeneric>>
    {
        public void Add(TGeneric o)
        {
            this.value ??= new List<TGeneric>();
            this.value.Add(o);

            this.valueChanged?.Invoke();
        }

        public void Remove(TGeneric o)
        {
            if (this.value == null)
                return;

            this.value.Remove(o);

            this.valueChanged?.Invoke();
        }
    }
}