using System;
using Cysharp.Threading.Tasks;

namespace ProjectName.InteractableModule.Runtime.Shared.Scripts.Interactables
{
    public abstract class ConcreteInteractablesFactory<TInteractableSerializableComponents,
        TInteractableInitalizationData, TInteractable> : IConcreteInteractablesFactory<
        TInteractableSerializableComponents,
        TInteractableInitalizationData, TInteractable>
        where TInteractableSerializableComponents : InteractableSerializableComponents
        where TInteractableInitalizationData : struct
        where TInteractable : IInteractable
    {
        public abstract UniTask<TInteractable> GetCreatedConcreteInteractableAsync(
            TInteractableSerializableComponents serializableComponents,
            TInteractableInitalizationData? data);

        public async UniTask<IInteractable> GetCreatedInteractableAsync(
            InteractableSerializableComponents interactableSerializableComponents,
            IInteractableInitializationData interactableInitializationData)
        {
            return await GetCreatedConcreteInteractableAsync(
                (TInteractableSerializableComponents)interactableSerializableComponents,
                interactableInitializationData is TInteractableInitalizationData data ? data : null);
        }

        public Type GetSerializableComponentsType()
        {
            return typeof(TInteractableSerializableComponents);
        }

        public Type GetInteractableType()
        {
            return typeof(TInteractable);
        }
    }
}