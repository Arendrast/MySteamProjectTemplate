using ProjectName.InteractableModule.Runtime.Shared.Scripts.Network;
using ProjectName.ItemModule.Runtime.Shared.Scripts.Logic;

namespace ProjectName.InteractableModule.Runtime.Shared.Scripts.Interactables
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