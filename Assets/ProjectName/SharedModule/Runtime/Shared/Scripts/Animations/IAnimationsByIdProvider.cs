using UnityEngine;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.Animations
{
    public interface IAnimationsByIdProvider
    {
        AnimationClip GetAnimation(int id);
    }
}