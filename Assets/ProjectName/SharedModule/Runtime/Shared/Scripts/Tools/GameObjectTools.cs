using System;
using UnityEngine;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.Tools
{
    public static class GameObjectTools
    {
        public static T GetOrAddComponent<T>(this GameObject instance) where T : Component
        {
            instance.TryGetComponent<T>(out var component);
            return component ? component : instance.AddComponent<T>();
        }

        public static T GetOrAddComponent<T>(this Component instance) where T : Component
        {
            return GetOrAddComponent<T>(instance.gameObject);
        }

        public static T GetComponentInParentsByPredicate<T>(this Component instance,
            Predicate<T> predicate = null, bool includeInactive = true) where T : Component
        {
            if (instance == null)
            {
                return null;
            }

            var myselfComponent = instance.GetComponent<T>();

            var isPredicateNull = predicate == null;

            if (myselfComponent != null && (isPredicateNull || predicate.Invoke(myselfComponent)))
            {
                return myselfComponent;
            }

            var currentTransform = instance.transform;

            while (true)
            {
                if ((includeInactive || currentTransform.gameObject.activeInHierarchy) && 
                    currentTransform.TryGetComponent<T>(out var component) &&
                    (isPredicateNull || predicate.Invoke(component)))
                {
                    return component;
                }

                currentTransform = currentTransform.parent;
                
                if (currentTransform == null)
                {
                    break;
                }
            }

            return null;
        }

        public static bool TryGetComponentInParentsByPredicate<T>(this Component instance, out T component,
            Predicate<T> predicate = null, bool includeInactive = true)
            where T : Component
        {
            component = instance.GetComponentInParentsByPredicate(predicate, includeInactive);
            return component != null;
        }
    }
}