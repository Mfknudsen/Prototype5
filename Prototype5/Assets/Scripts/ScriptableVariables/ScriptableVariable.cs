using UnityEngine;
using UnityEngine.Events;

namespace ScriptableVariables
{
    public abstract class ScriptableVariable<TGeneric> : ScriptableObject
    {
        protected TGeneric value;

        protected UnityEvent valueChanged;

#if UNITY_EDITOR
        [SerializeField] protected bool Debug;

        [SerializeField] [TextArea] private string description;
#endif

        public TGeneric Value
        {
            get => this.value;
            set
            {
                if (this.value.Equals(value))
                    return;

                this.value = value;
                this.valueChanged?.Invoke();
#if UNITY_EDITOR
                if (this.Debug)
                    UnityEngine.Debug.Log(value, this);
#endif
            }
        }

        public void AddListener(UnityAction action)
        {
            this.valueChanged ??= new UnityEvent();

            this.valueChanged.AddListener(action);
        }

        public void RemoveListener(UnityAction action)
        {
            this.valueChanged?.RemoveListener(action);
        }
    }
}