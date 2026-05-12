using System.Linq;
using FishNet.Broadcast;
using FishNet.Object;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Tools;

namespace ProjectName.InteractableModule.Runtime.Shared.Scripts.Network.Broadcasts
{
    public struct InitializeInteractablesBroadcast : IBroadcast
    {
        public readonly InteractableData[] Data;
        public readonly bool[] CanInteractByInteractable;
        
        
        public InitializeInteractablesBroadcast(InteractableData[] data, bool[] canInteractByInteractable)
        {
            Data = data;
            CanInteractByInteractable = canInteractByInteractable;
        }

        public override string ToString()
        {
            return Data.Select(data => data.NetworkObjectId != NetworkObject.UNSET_SCENEID_VALUE ? data.ToString() : "null").JoinString();
        }
    }
}