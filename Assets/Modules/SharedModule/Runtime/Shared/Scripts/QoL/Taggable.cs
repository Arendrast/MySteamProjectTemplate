using System;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.QoL
{
    public enum CustomTag
    {
        Knockout,
        Reviving,
        Died,
        
        Head,
        
        SoftSurface,
        SlidingSurface,
        Revived,
    }
    
    public class Taggable : MonoBehaviour
    {
        public IReadOnlyList<CustomTag> Tags => _tags;
        
        public event Action<CustomTag> Added, Removed;
        [SerializeField] private List<CustomTag> _tags = new();

        public bool HasTag(CustomTag tag)
        {
            return _tags.Contains(tag);
        }

        public void TryAddTag(CustomTag tag)
        {
            if (!HasTag(tag))
            {
                _tags.Add(tag);
                Added?.Invoke(tag);
            }
        }

        public void RemoveTag(CustomTag tag)
        {
            if (_tags.Remove(tag))
            {
                Removed?.Invoke(tag);
            }
        }
    }

    public static class TaggableTools
    {
        public static bool HasCustomTag(this Component component, CustomTag tag)
        {
            return component != null && HasCustomTag(component.gameObject, tag);
        }
        
        public static bool HasCustomTag(this GameObject gameObject,  CustomTag tag)
        {
            return gameObject != null &&
                   gameObject.TryGetComponent<Taggable>(out var taggable) &&
                   taggable.HasTag(tag);
        }
    }
}