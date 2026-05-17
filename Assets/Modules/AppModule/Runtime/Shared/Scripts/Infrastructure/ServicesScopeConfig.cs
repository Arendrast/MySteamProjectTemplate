using System;
using UnityEngine;

namespace Modules.AppModule.Runtime.Shared.Scripts.Infrastructure
{
    [Serializable]
    public struct ServicesScopeConfig
    {
        [field: SerializeField] public bool ShouldUseBuildMode { get; private set; }
    }
}