using UnityEngine;

namespace NPCs
{
    public class NpcStateMachine<T> : MonoBehaviour where T : NpcStateMachine<T>
    {
        protected NpcState<T> currentState;

        public void SwitchState(NpcState<T> npcState)
        {
            currentState?.Exit();
            currentState = npcState;
            currentState.Enter();
        }

        protected void Update() => currentState?.UpdateLogic();
        protected void FixedUpdate() => currentState?.UpdatePhysics();
    }
}
