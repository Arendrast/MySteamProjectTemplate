using System;
using UnityEngine;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.QoL
{
    [Serializable]
    public class TransformConfig
    {
        [field: SerializeField] public Vector3 Position { get; private set; }
        [field: SerializeField] public Transform PositionTransform { get; private set; }
        [field: SerializeField] public Vector3 Rotation { get; private set; }
        [field: SerializeField] public Transform Parent { get; private set; }
    } 
}