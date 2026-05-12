using System;
using Cysharp.Threading.Tasks;
using Modules.SharedModule.Runtime.Shared.Scripts.QoL;

namespace Modules.InteractableModule.Runtime.Shared.Scripts.Interactables
{
    public interface IConcreteInteractablesSerializableComponentsFactory : IMatchSharedFactory
    {
        UniTask<InteractableSerializableComponents> GetCreatedSerializableComponentsAsync(
            IInteractableInitializationData interactableInitializationData);

        Type GetDataType();
    }

    public interface IConcreteInteractablesSerializableComponentsFactory<TInteractableInitializationData> :
        IConcreteInteractablesSerializableComponentsFactory where TInteractableInitializationData : struct
    {
        UniTask<InteractableSerializableComponents> GetCreatedConcreteSerializableComponentsAsync(
            TInteractableInitializationData interactableInitializationData);
    }
}