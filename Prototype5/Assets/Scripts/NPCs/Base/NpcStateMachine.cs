using UnityEngine;

namespace NPCs.Base
{
    public class NpcStateMachine<T> : MonoBehaviour where T : NpcStateMachine<T>
    {
        protected NpcState<T> currentState;
        protected internal NpcState<T> previousState;
        protected internal Animator animator;

        public void SwitchState(NpcState<T> npcState)
        {
            if (currentState == npcState) return;

            previousState = currentState ?? npcState;
            
            currentState?.Exit();
            currentState = npcState;
            currentState.Enter();
        }

        protected void Update() => currentState?.UpdateLogic();
        protected void FixedUpdate() => currentState?.UpdatePhysics();
    }
}
