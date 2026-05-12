namespace ProjectName.ItemModule.Runtime.Shared.Scripts.Logic
{
    public enum ItemType
    {
        None = 0,
        
        MainWeapon = 1,
        Pistol = 2,
        MeleeWeapon = 3,

        FragmentationGrenade = 4,
        BaitGrenade = 5,
        Molotov = 8,

        Kit = 6,
        Equipment = 7,
        Can = 9
    }
    
    public static class ItemTypeTools
    {
        public static bool IsKit(this ItemType itemType)
        {
            return itemType is ItemType.Kit;
        }
        
        public static bool IsGrenade(this ItemType itemType)
        {
            return itemType is ItemType.BaitGrenade or ItemType.FragmentationGrenade
                or ItemType.Molotov;
        }

        public static bool IsGrenadeOrKitOrCan(this ItemType itemType)
        {
            return IsKit(itemType) || IsGrenade(itemType) || IsCan(itemType);
        }

        public static bool IsCan(this ItemType itemType)
        {
            return itemType is ItemType.Can;
        }
    }
}