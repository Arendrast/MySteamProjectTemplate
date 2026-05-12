namespace ProjectName.NetworkModule.Runtime.Shared.Scripts.Synchronizers.SynchronizerBehaviours
{
    public interface IValueUpdater<TValue>
    {
        void UpdateValueAsync(TValue value);
    }
}