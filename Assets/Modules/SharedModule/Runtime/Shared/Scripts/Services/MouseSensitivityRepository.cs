namespace Modules.SharedModule.Runtime.Shared.Scripts.Services
{
    public class MouseSensitivityRepository : IPersistentService
    {
        public float CurrentSensitivity { get; private set; } = 2f;

        public void SetCurrentSensitivity(float currentSensitivity)
        {
            CurrentSensitivity = currentSensitivity;
        }
    }
}