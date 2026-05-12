using System;
using Modules.SharedModule.Runtime.Shared.Scripts.Services;
using UnityEngine;

namespace Modules.NetworkModule.Runtime.Shared.Scripts.RigidbodySync
{
    public class RigidbodySynchronizationService : IMatchSharedService
    {
        public event Action<Vector3, int, ForceMode> SentAddForceData;
        
        public void SendAddForceData(Vector3 force, int networkObjectId, ForceMode forceMode = ForceMode.Force)
        {
            SentAddForceData?.Invoke(force, networkObjectId, forceMode);
        }
    }
}