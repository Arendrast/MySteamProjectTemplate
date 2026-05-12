namespace ProjectName.EntityModule.Runtime.Shared.Scripts.Heal
{
    public enum HealOrigin
    {
        TeammatesKit,
        MyselfKit,
        KitHit,
        Revival
    }

    public static class HealReasonTools
    {
        public static bool IsKit(this HealOrigin? healReason)
        {
            return healReason.HasValue && IsKit(healReason.Value);
        }
        public static bool IsKit(this HealOrigin healOrigin)
        {
            return healOrigin is HealOrigin.TeammatesKit or HealOrigin.MyselfKit or HealOrigin.KitHit;
        }
    }
}