using System.Linq;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.QoL
{
    public class TransformAligner: MonoBehaviour
    {
        [Button]
        private void AlignTransform()
        {
            transform.Align(transform.Cast<Transform>().ToArray());
        }
    }
}