namespace NPCs
{
    public class NpcIdleState : NpcState
    {
        private const float wanderExtraDistance = 2.0f;
        public NpcIdleState(NpcBehaviour npcBehaviour) : base(npcBehaviour) {}

        public override void Enter()
        {
            npcBehaviour.agent.isStopped = true;
        }

        public override void UpdateLogic()
        {
            if (npcBehaviour.DistanceToTarget >= npcBehaviour.maxDistanceToTarget + wanderExtraDistance)
                npcBehaviour.SwitchState(npcBehaviour.wanderState);
        }
    }
}
