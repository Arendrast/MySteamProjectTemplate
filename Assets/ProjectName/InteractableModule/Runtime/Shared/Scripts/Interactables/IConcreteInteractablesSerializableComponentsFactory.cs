using System;
using Cysharp.Threading.Tasks;
using ProjectName.SharedModule.Runtime.Shared.Scripts.QoL;

namespace ProjectName.InteractableModule.Runtime.Shared.Scripts.Interactables
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