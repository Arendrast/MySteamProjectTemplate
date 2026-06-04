using System;
using UnityEngine;

namespace Modules.OverlapModule.Runtime.Scripts
{
    public class OverlapGizmosDrawer
    {
        private readonly OverlapObserverDebugConfig _config;
        private readonly Action<bool> _drawGizmos;

        public OverlapGizmosDrawer(OverlapObserverDebugConfig config, Action<bool> drawGizmos)
        {
            _config = config;
            _drawGizmos = drawGizmos;
        }

        public void DrawGizmos(bool selected)
        {
            var gizmosColor = Gizmos.color;
            Gizmos.color = selected ? _config.SelectedColor : _config.DefaultColor;
            _drawGizmos.Invoke(selected);
            Gizmos.color = gizmosColor;
        }
    }
}