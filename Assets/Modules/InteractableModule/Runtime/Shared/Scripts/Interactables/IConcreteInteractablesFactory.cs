using System;
using Cysharp.Threading.Tasks;
using Modules.SharedModule.Runtime.Shared.Scripts.QoL;

namespace Modules.InteractableModule.Runtime.Shared.Scripts.Interactables
{
    public interface IConcreteInteractablesFactory : IMatchSharedFactory
    {
        UniTask<IInteractable> GetCreatedInteractableAsync(
            InteractableSerializableComponents interactableSerializableComponents,
            IInteractableInitializationData interactableInitializationData);

        Type GetSerializableComponentsType();
        Type GetInteractableType();
    }

    public interface IConcreteInteractablesFactory<TInteractableSerializableComponents,
        TInteractableInitalizationData, TInteractable> : IConcreteInteractablesFactory
        where TInteractableSerializableComponents : InteractableSerializableComponents
        where TInteractableInitalizationData : struct
        where TInteractable : IInteractable
    {
        UniTask<TInteractable> GetCreatedConcreteInteractableAsync(
            TInteractableSerializableComponents serializableComponents,
            TInteractableInitalizationData? data);
    }
}