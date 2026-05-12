namespace Modules.SharedModule.Runtime.Client.Scripts.Animator
{
    public interface IExitedAnimatorStateReader
    {
        void OnExitState(int stateHash);
        void OnEndState(int stateHash);
    }
}