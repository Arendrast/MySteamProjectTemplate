namespace Modules.InteractableModule.Runtime.Shared.Scripts.Network.Broadcasts
{
    public readonly struct InteractableData
    {
        public readonly int NetworkObjectId;
        public readonly string SerializedConcreteInteractableData; 

        public InteractableData(int networkObjectId, string serializedConcreteInteractableData)
        {
            NetworkObjectId = networkObjectId;
            SerializedConcreteInteractableData = serializedConcreteInteractableData;
        }
    }
}