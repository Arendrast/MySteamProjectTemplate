using System.Linq;
using MoreLinq;
using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Tools
{
    public static class LayerMaskTools
    {
        public static int ToSingleLayer(this RenderingLayerMask layerMask)
        {
            var value = layerMask.value;

            if (value == 0)
                return -1;

            return Mathf.FloorToInt((value & (value - 1)) != 0 ? Mathf.Log(value & -value, 2) : Mathf.Log(value, 2));
        }

        public static LayerMask Without(this RenderingLayerMask layerMask, int layer)
        {
            return layerMask | ~(1 << layer);
        }
        
        public static LayerMask Without(this RenderingLayerMask layerMask, LayerMask layerMask2)
        {
            return layerMask & (~layerMask2);
        }

        public static LayerMask Concat(this RenderingLayerMask layerMask, LayerMask layerMask2)
        {
            return layerMask | layerMask2;
        }
        
        public static bool DoesHaveLayer(this RenderingLayerMask layerMask, int layer)
        {
            return (layerMask.value & 1 << layer) != 0;
        }
        
        public static bool DoesHaveLayers(this RenderingLayerMask layerMask, int[] layers)
        {
            return layers.Any(layer => DoesHaveLayer(layerMask, layer));
        }

        public static RenderingLayerMask GetEverythingRenderingLayersExcept(params int[] layers)
        {
            LayerMask mask = 0;
            layers.ForEach(layer => mask |= 1 << layer);
            return ~mask;
        }
        
        public static int ToSingleLayer(this LayerMask layerMask)
        {
            var value = layerMask.value;

            if (value == 0)
                return -1;

            return Mathf.FloorToInt((value & (value - 1)) != 0 ? Mathf.Log(value & -value, 2) : Mathf.Log(value, 2));
        }

        public static LayerMask Without(this LayerMask layerMask, int layer)
        {
            return layerMask | ~(1 << layer);
        }
        
        public static LayerMask Without(this LayerMask layerMask, LayerMask layerMask2)
        {
            return layerMask & (~layerMask2);
        }

        public static LayerMask Concat(this LayerMask layerMask, LayerMask layerMask2)
        {
            return layerMask | layerMask2;
        }
        
        public static bool DoesHaveLayer(this LayerMask layerMask, int layer)
        {
            return (layerMask.value & 1 << layer) != 0;
        }
        
        public static bool DoesHaveLayers(this LayerMask layerMask, int[] layers)
        {
            return layers.Any(layer => DoesHaveLayer(layerMask, layer));
        }

        public static LayerMask GetEverythingPhysicsLayersExcept(params int[] layers)
        {
            LayerMask mask = 0;
            layers.ForEach(layer => mask |= 1 << layer);
            return ~mask;
        }
    }
}