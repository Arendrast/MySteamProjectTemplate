using System;
using Cysharp.Threading.Tasks;

namespace ProjectName.InteractableModule.Runtime.Shared.Scripts.Interactables
{
    public abstract class ConcreteInteractablesSerializableComponentsFactory<TInteractableInitializationData> :
        IConcreteInteractablesSerializableComponentsFactory<TInteractableInitializationData>
        where TInteractableInitializationData : struct
    {
        public abstract UniTask<InteractableSerializableComponents> GetCreatedConcreteSerializableComponentsAsync(
            TInteractableInitializationData interactableInitializationData);

        public async UniTask<InteractableSerializableComponents> GetCreatedSerializableComponentsAsync(
            IInteractableInitializationData interactableInitializationData)
        {
            return await GetCreatedConcreteSerializableComponentsAsync(
                (TInteractableInitializationData) interactableInitializationData);
        }

        public Type GetDataType()
        {
            return typeof(TInteractableInitializationData);
        }
    }
}