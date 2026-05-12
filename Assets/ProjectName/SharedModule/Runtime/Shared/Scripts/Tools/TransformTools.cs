using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.Tools
{
    public static class TransformTools
    {
        public static float SqrMagnitudeTo(this Transform t, Transform o)
        {
            return (t.position - o.position).sqrMagnitude;
        }
        
        public static void Align(this Transform transform, IReadOnlyList<Transform> children)
        {
            var positions = children.Select(x => x.transform.position).ToArray();
            var center = positions.Aggregate((v1, v2) => v1 + v2) / children.Count;

            transform.position = center;
            
            for (var i = 0; i < children.Count; i++)
            {
                var position = positions[i];
                var enemy = children[i];

                enemy.transform.position = position;
            }
        }
    }
}