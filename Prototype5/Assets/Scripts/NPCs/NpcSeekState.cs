namespace NPCs
{
    public class NpcSeekState : NpcState
    {
        public NpcSeekState(NpcBehaviour npcBehaviour) : base(npcBehaviour) {}

        public override void Enter()
        {
            npcBehaviour.agent.isStopped = false;
        }

        public override void UpdateLogic()
        {
            if (npcBehaviour.DistanceToTarget < npcBehaviour.minDistanceToTarget)
                npcBehaviour.SwitchState(npcBehaviour.idleState);
            else if (npcBehaviour.DistanceToTarget >= npcBehaviour.maxDistanceToTarget)
                npcBehaviour.SwitchState(npcBehaviour.wanderState);
        }

        public override void UpdatePhysics()
        {
            SeekTarget();
        }

        private void SeekTarget()
        {
            npcBehaviour.agent.SetDestination(npcBehaviour.targetTransform.position);
        }
    }
}
