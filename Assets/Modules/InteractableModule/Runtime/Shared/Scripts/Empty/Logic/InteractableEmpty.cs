using System;
using Cysharp.Threading.Tasks;
using FishNet.Connection;
using Modules.InteractableModule.Runtime.Shared.Scripts.Interactables;
using Modules.InteractableModule.Runtime.Shared.Scripts.Network;
using Modules.InteractableModule.Runtime.Shared.Scripts.Network.Visitors;
using Modules.SharedModule.Runtime.Shared.Scripts.QoL;

namespace Modules.InteractableModule.Runtime.Shared.Scripts.Empty.Logic
{
    public class InteractableEmpty : IServerSyncableInteractable, IClientSyncableInteractable, IRemovableInteractable
    {
        public bool CanInteract { get; set; } = true;
        
        public event Action Interacted;

        public void Accept(IOwnerInteractionVisitor visitor, IFromServerInteractionData interactionData)
        {
            Interacted?.Invoke();
        }

        public void Accept(INotOwnerInteractionVisitor visitor, IAdditionalInteractionData additionalInteractionData)
        {
            Interacted?.Invoke();
        }

        public IFromServerInteractionData AcceptAndGetFromServerInteractionData(NetworkConnection networkConnection,
            IAdditionalInteractionData data)
        {
            return null;
        }

        public bool ShouldBeRemoved()
        {
            return false;
        }

        public bool CanAccept(IAdditionalInteractionData data)
        {
            return true;
        }

        public IInteractableInitializationData GetInitializationData()
        {
            return null;
        }

        public UniTask<bool> CanLocalAcceptAsync(IOwnerInteractionVisitor visitor,
            DataContainer<IAdditionalInteractionData> additionalInteractionData)
        {
            return UniTask.FromResult(true);
        }
    }
}