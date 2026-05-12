namespace ProjectName.SharedModule.Runtime.Shared.Scripts.Holders
{
    public static class PlayersPrefsVariablesNamesHolder
    {
        public const string MouseSensitivity = "MouseSensitivity";
        public const string HostSteamId = "HostSteamId";
        public const string TargetLevelNumber = "TargetLevelNumber";

        public static string GetAudioVolume(string audioType)
        {
            return audioType + "Volume";
        }
    }
}