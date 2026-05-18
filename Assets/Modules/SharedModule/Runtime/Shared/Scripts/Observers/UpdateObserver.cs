using System;
using Modules.SharedModule.Runtime.Shared.Scripts.QoL;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Observers
{
    public class UpdateObserver
    {
        public UpdateType UpdateType { get; }
        public GameObject GameObject { get; }
        public EnableDisableObserver EnableDisableObserver { get; }
        public EnableStateProvider EnableStateProvider { get; }
        
        public event Action<float> Updated;

        public UpdateObserver(UpdateType updateType, GameObject gameObject)
        {
            UpdateType = updateType;
            GameObject = gameObject;
            EnableDisableObserver = GameObject.GetOrAddComponent<EnableDisableObserver>();
            EnableStateProvider = new EnableStateProvider(gameObject.activeInHierarchy);

            EnableDisableObserver.Enabled += SetEnableStateToTrue;
            EnableDisableObserver.Disabled += SetEnableStateToFalse;
            
            EnableStateProvider.SetEnableState(GameObject.activeInHierarchy);
            
            return;
            
            void SetEnableStateToTrue()
            {
                EnableStateProvider.SetEnableState(true);
            }

            void SetEnableStateToFalse()
            {
                EnableStateProvider.SetEnableState(false);
            }
        }

        public void Update(float time)
        {
            Updated?.Invoke(time);
        }
    }
}