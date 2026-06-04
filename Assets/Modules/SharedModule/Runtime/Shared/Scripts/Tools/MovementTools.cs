using System;
using Animancer.Units;
using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Tools
{
    public static class MovementTools
    {
        public static void MoveTowards(Vector3 direction, float maxSpeed, float accelerationPerSecond, float deltaTime, Transform transform, ref Vector3 currentVelocity)
        {
            var targetVelocity = direction.normalized * maxSpeed;
                
            currentVelocity = Vector3.MoveTowards(
                currentVelocity, 
                targetVelocity, 
                accelerationPerSecond * deltaTime
            );
                
            transform.position += currentVelocity * deltaTime;
        }
    }
}