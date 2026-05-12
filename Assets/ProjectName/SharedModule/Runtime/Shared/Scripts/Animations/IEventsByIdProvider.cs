namespace ProjectName.SharedModule.Runtime.Shared.Scripts.Animations
{
    public interface IEventsByIdProvider
    {
        AnimancerStateEventsConfig GetEvents(int id);
    }
}