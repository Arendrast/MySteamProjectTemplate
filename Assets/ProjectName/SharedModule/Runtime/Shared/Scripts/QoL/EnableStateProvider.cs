namespace ProjectName.SharedModule.Runtime.Shared.Scripts.QoL
{
    public class EnableStateProvider
    {
        public bool IsEnable { get; private set; }

        public EnableStateProvider(bool isEnable = true) => IsEnable = isEnable;

        public void SetEnableState(bool isEnable) => IsEnable = isEnable;
    }
}