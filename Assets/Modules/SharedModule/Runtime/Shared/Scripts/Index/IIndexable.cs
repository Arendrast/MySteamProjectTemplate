namespace Modules.SharedModule.Runtime.Shared.Scripts.Index
{
    public interface IIndexable: IIndexable<int>
    {
    }

    public interface IIndexable<out TId>
    {
        TId Id { get; }
    }
}