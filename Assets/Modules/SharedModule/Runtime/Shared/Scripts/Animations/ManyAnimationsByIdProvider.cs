using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Animations
{
    [Serializable]
    public class ManyAnimationsByIdProvider : IAnimationsByIdProvider
    {
        [Serializable]
        private class Pair
        {
            [field: SerializeField] public AnimationClip Clip { get; private set; }
            [field: SerializeField] public int Id { get; private set; }
        }

        [SerializeField] private List<Pair> _pairs;
        
        public AnimationClip GetAnimation(int id)
        {
            return _pairs.FirstOrDefault(pair => pair.Id == id)?.Clip;
        }
    }
}