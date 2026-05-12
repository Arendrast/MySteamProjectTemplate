namespace ProjectName.SharedModule.Runtime.Shared.Scripts.EventBusSystem.Events
{
    public struct SetOpenStateHudPopupEvent : IEvent
    {
        public readonly bool IsOpen;

        public SetOpenStateHudPopupEvent(bool isOpen)
        {
            IsOpen = isOpen;
        }
    }
}