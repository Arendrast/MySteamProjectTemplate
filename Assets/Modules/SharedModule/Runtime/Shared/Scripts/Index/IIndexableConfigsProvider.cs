namespace Modules.SharedModule.Runtime.Shared.Scripts.Index
{
    public interface IIndexableConfigsProvider<TIndexable> where TIndexable : IIndexable
    {
        TIndexable[] Configs { get; }
    }
}