using Modules.InteractableModule.Runtime.Shared.Scripts.Network;
using Modules.ItemModule.Runtime.Shared.Scripts.Logic;

namespace Modules.InteractableModule.Runtime.Shared.Scripts.Interactables
{
    public struct ItemInSlotCreateData : IAdditionalInteractionData
    {
        public readonly ICreateConcreteItemData CreateConcreteItemData;

        public ItemInSlotCreateData(ICreateConcreteItemData createConcreteItemData)
        {
            CreateConcreteItemData = createConcreteItemData;
        }
    }
}