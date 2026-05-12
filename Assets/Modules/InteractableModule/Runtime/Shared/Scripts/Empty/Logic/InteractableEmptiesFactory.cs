using Cysharp.Threading.Tasks;
using Modules.InteractableModule.Runtime.Shared.Scripts.Interactables;

namespace Modules.InteractableModule.Runtime.Shared.Scripts.Empty.Logic
{
    public class InteractableEmptiesFactory : ConcreteInteractablesFactory<
        InteractableEmptySerializableComponents, InteractableEmptyInitializationData,
        InteractableEmpty>
    {
        public override UniTask<InteractableEmpty> GetCreatedConcreteInteractableAsync(
            InteractableEmptySerializableComponents serializableComponents,
            InteractableEmptyInitializationData? data)
        {
            return UniTask.FromResult(new InteractableEmpty());
        }
    }
}