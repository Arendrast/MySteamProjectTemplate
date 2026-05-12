using FishNet;
using TMPro;
using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.UI
{
    public class PingView : MonoBehaviour
    {
        /// <summary>
        /// True to show the real ping. False to include tick rate latency within the ping.
        /// </summary>
        [Tooltip("True to show the real ping. False to include tick rate latency within the ping.")] [SerializeField]
        private bool _hideTickRate = true;

        [SerializeField] private TMP_Text _text;

        private float _interval = 0.5f;
        private float _time;

        private void Update()
        {
            _time += Time.deltaTime;

            if (_time <= _interval)
                return;
            
            var ping = InstanceFinder.TimeManager.RoundTripTime;
            long deduction = 0;

            if (_hideTickRate)
                deduction = (long)(InstanceFinder.TimeManager.TickDelta * 2000d);

            ping = (long)Mathf.Max(1, ping - deduction);
            _text.text = $"{ping:F0}";
            _time = 0;
        }
    }
}