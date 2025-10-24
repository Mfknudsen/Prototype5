using System.Collections.Generic;
using Listeners;
using UnityEngine;

namespace ScriptableVariables.Objects
{
    [CreateAssetMenu(fileName = "GameEvent", menuName = "Scriptable Objects/GameEvent")]
    
    public class GameEvent : ScriptableObject
    {
        private List<GameEventListener> _listeners = new List<GameEventListener>();

        public void AddListener(GameEventListener listener)
        {
            _listeners.Add(listener);
        }

        public void RemoveListener(GameEventListener listener)
        {
            _listeners.Remove(listener);
        }

        public void InvokeGameEvents()
        {
            for (int i = _listeners.Count - 1; i >= 0; i--)
            {
                _listeners[i].OnEventInvoke();
            }
        }
        
    }
}
