namespace Interactions
{
    public interface IInteractable
    {
        //Returns the time the interaction takes, 1 = 1 second
        public float OnTrigger();
    }
}