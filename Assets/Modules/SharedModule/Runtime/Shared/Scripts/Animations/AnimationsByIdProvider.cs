using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Animations
{
    [Serializable]
    public abstract class AnimationsByIdProvider<T> : IAnimationsByIdProvider where T : Enum
    {
        [Serializable]
        public class Pair
        {
            [field: SerializeField] public T Name { get; private set; }
            [field: SerializeField] public AnimationClip AnimationClip { get; private set; }
        }

        [field: SerializeField] private List<Pair> _animationsByType;
        
        public AnimationClip GetAnimation(int id)
        {
            return _animationsByType.FirstOrDefault(pair => (int)(object)pair.Name == id)?.AnimationClip;
        }
    }
}