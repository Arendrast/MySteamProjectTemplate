using System;
using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Animations
{
    [Serializable]
    public class SingleAnimationByIdProvider : IAnimationsByIdProvider
    {
        [SerializeField] private AnimationClip _animationClip;
        
        public AnimationClip GetAnimation(int id)
        {
            return _animationClip;
        }
    }
}