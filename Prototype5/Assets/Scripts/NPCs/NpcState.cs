namespace NPCs
{
    public abstract class NpcState
    {
        protected NpcBehaviour npcBehaviour;

        public NpcState(NpcBehaviour npcBehaviour) => this.npcBehaviour = npcBehaviour;
        
        public virtual void Enter() {}
        public virtual void UpdateLogic() {}
        public virtual void UpdatePhysics() {}
        public virtual void Exit() {}

    }
}
