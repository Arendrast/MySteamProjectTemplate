namespace ProjectName.SharedModule.Runtime.Shared.Scripts.Index
{
    public static class IndexableTools
    {
        public const int MissingOrInvalidId = -1;
        public static bool IsValidId(int id) => id >= 0;
        public static bool IsValidId(this IIndexable indexable) => IsValidId(indexable.Id);
    }
}