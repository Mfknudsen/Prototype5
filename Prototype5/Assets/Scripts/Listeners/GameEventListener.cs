using ScriptableVariables.Objects;
using UnityEngine;
using UnityEngine.Events;

namespace Listeners
{
    public class GameEventListener : MonoBehaviour
    {
        public GameEvent gameEvent;
        public UnityEvent response;

        private void OnEnable()
        {
            gameEvent.AddListener(this);
        }

        private void OnDisable()
        {
            gameEvent.RemoveListener(this);
        }

        public void OnEventInvoke()
        {
            response.Invoke();
        }
    }
}
