using UnityEngine;

namespace Modules.ItemModule.Runtime.Shared.Scripts.Logic
{
    public interface IItemModel
    {
        IItemConfig Config { get; }
        GameObject LogicGameObject { get; }
        bool CanInterruptLogic(InterruptReason interruptReason);
        bool IsUsing();
        void InterruptUsing();
        ICreateConcreteItemData GetConcreteItemData();
    }

    public enum InterruptReason
    {
        Reload,
        Use,
        SetItem,
        Throw,
        Remove,
        Any
    }
}