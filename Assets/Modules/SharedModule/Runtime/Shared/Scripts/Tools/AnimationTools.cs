namespace Modules.SharedModule.Runtime.Shared.Scripts.Tools
{
    public static class AnimationTools
    {
        public static float FromFramesToSeconds(this int frame, int numberFramesPerSecond = 30)
        {
            return 1f / numberFramesPerSecond * frame;
        }
    }
}