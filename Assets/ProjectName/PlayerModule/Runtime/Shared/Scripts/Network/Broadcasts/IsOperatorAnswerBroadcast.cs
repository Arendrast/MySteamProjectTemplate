using FishNet.Broadcast;

namespace ProjectName.PlayerModule.Runtime.Shared.Scripts.Network.Broadcasts
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