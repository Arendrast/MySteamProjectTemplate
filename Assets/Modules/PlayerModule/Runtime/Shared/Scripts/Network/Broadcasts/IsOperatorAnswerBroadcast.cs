using FishNet.Broadcast;

namespace Modules.PlayerModule.Runtime.Shared.Scripts.Network.Broadcasts
{
    public struct IsOperatorAnswerBroadcast : IBroadcast
    {
        public readonly bool IsOperator;

        public IsOperatorAnswerBroadcast(bool isOperator)
        {
            IsOperator = isOperator;
        }
    }
}