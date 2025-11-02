using UnityEngine;
using UnityEngine.Events;

namespace ScriptableVariables
{
    public abstract class ScriptableVariable<TGeneric> : ScriptableObject
    { 
        protected TGeneric value;

        protected UnityEvent<TGeneric> valueChanged;

#if UNITY_EDITOR
        [SerializeField] protected bool Debug;

        [SerializeField] [TextArea] private string description;
#endif

        public TGeneric Value
        {
            get => this.value;
            set
            {
                if (this.value != null && this.value.Equals(value))
                    return;

                this.value = value;
                this.valueChanged?.Invoke(value);

#if UNITY_EDITOR
                if (this.Debug)
                    UnityEngine.Debug.Log(value, this);
#endif
            }
        }

        public void AddListener(UnityAction<TGeneric> action)
        {
            this.valueChanged ??= new UnityEvent<TGeneric>();

            this.valueChanged.AddListener(action);
        }

        public void RemoveListener(UnityAction<TGeneric> action)
        {
            this.valueChanged?.RemoveListener(action);
        }
    }
}