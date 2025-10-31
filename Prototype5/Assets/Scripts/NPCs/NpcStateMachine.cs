using UnityEngine;

namespace NPCs
{
    public class NpcStateMachine : MonoBehaviour
    {
        protected NpcState currentState;

        public void SwitchState(NpcState npcState)
        {
            currentState?.Exit();
            currentState = npcState;
            currentState.Enter();
        }

        protected void Update() => currentState?.UpdateLogic();
        protected void FixedUpdate() => currentState?.UpdatePhysics();
    }
}
