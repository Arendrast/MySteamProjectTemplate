using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Animations
{
    public interface IAnimationsByIdProvider
    {
        AnimationClip GetAnimation(int id);
    }
}