using FishNet.Object;
using ProjectName.SharedModule.Runtime.Shared.Scripts.QoL;
using UnityEngine;

namespace ProjectName.NetworkModule.Runtime.Shared.Scripts.Synchronizers
{
    public class LoopedSetActiveSynchronizer : NetworkBehaviour
    {
        [SerializeField] private NetworkSetActiveSynchronizer _networkSetActiveSynchronizer;
        [SerializeField] private float _timeBeforeEnable;
        [SerializeField] private float _timeBeforeDisable;
        [SerializeField] private float _timeBeforeFirstSetActive;
         
        private Timer _timer;
        
        public override void OnStartNetwork()
        {
            if (!IsServerInitialized)
                return;
            
            _timer = new Timer(destroyCancellationToken);

            _timer.Ended += SetActiveAndRestartTimer;

            _timer.TryStartCountingTime(_timeBeforeFirstSetActive);
            
            return;
            
            void SetActiveAndRestartTimer()
            {
                _networkSetActiveSynchronizer.TrySetActiveState(!gameObject.activeSelf);
                _timer.TryStartCountingTime(gameObject.activeSelf ? _timeBeforeDisable : _timeBeforeEnable);
            }
        }
    }
}