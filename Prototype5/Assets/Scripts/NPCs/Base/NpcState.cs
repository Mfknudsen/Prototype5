namespace NPCs.Base
{
    public abstract class NpcState<T> where T : NpcStateMachine<T>
    {
        protected T fsm;

        public NpcState(T fsm) => this.fsm = fsm;
        
        public virtual void Enter() {}
        public virtual void UpdateLogic() {}
        public virtual void UpdatePhysics() {}
        public virtual void Exit() {}

    }
}
