using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.CameraPart
{
    public class CameraSerializableComponents : MonoBehaviour
    {
        [Serializable]
        private class Pair
        {
            [field: SerializeField] public CameraParentType CameraParentType { get; private set; }
            [field: SerializeField]
            public Transform Transform
            {
                get;
                private set;
            }
        }
        
        public IReadOnlyDictionary<CameraParentType, Transform> CameraParentsTransformsByType
             => _cameraParentsTransformsByType.ToDictionary(
            pair => pair.CameraParentType, pair => pair.Transform);
        
        public Transform this[CameraParentType type] => CameraParentsTransformsByType[type];
        
        [field: SerializeField] public PushedCameraSerializableComponents PushedCameraSerializableComponents { get; private set; }
        [field: SerializeField] public FPSCameraSerializableComponents FPSCameraSerializableComponents { get; private set; }
        [field: SerializeField] public UnityEngine.Camera Camera { get; private set; }
        [SerializeField] private List<Pair> _cameraParentsTransformsByType;
    }

    public enum CameraParentType
    {
        Move,
        Shake,
        Animation
    }
}