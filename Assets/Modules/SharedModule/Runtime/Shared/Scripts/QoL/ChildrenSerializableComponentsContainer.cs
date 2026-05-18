using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.QoL
{
    public class ChildrenSerializableComponentsContainer : MonoBehaviour, ISerializationCallbackReceiver
    {
        [SerializeField] private List<Component> _containedComponents = new List<Component>();

        private bool _cacheNeedsRebuild = true;
        private readonly Dictionary<Type, List<Component>> _polymorphicCache = new Dictionary<Type, List<Component>>();

        [ContextMenu("Editor: Bake Components In Children")]
        public void BakeComponentsInEditor()
        {
            var allComponentsInHierarchy = GetComponentsInChildren<Component>(true)
                .Where(c => c is MonoBehaviour and not ChildrenSerializableComponentsContainer) // Фильтруем мусор
                .ToList(); 

            ClearAndAddComponents(allComponentsInHierarchy);

#if UNITY_EDITOR
            Debug.Log($"[Editor Bake] Запечено {_containedComponents.Count} компонентов в {gameObject.name}");
#endif
        }

        public void ClearContainedComponents()
        {
            _containedComponents.Clear();
        }

        public void RegisterComponent(Component comp)
        {
            if (comp == null)
            {
                return;
            }
            
            if (!_containedComponents.Contains(comp))
            {
                _containedComponents.Add(comp);
                AddToCache(comp); 
            }
        }

        public void UnregisterComponent(Component comp)
        {
            if (comp == null)
            {
                return;
            }
            
            if (_containedComponents.Remove(comp))
            {
                RemoveFromCache(comp); 
            }
        }

        public void OnAfterDeserialize()
        {
            RebuildCache();
        }

        public void OnBeforeSerialize()
        {
        }
        
        public List<T> GetContainedChildren<T>() where T : Component
        {
            RebuildCacheIfNeeded();

            Type targetType = typeof(T);
            if (_polymorphicCache.TryGetValue(targetType, out var list))
            {
                return list.Cast<T>().ToList();
            }

            return new List<T>(); 
        }

        private void MarkCacheDirty()
        {
            _cacheNeedsRebuild = true;
        }

        private void RebuildCacheIfNeeded()
        {
            if (_cacheNeedsRebuild)
            {
                RebuildCache();
                _cacheNeedsRebuild = false;
            }
        }

        public void RebuildCache()
        {
            _polymorphicCache.Clear();
            
            foreach (var comp in _containedComponents)
            {
                AddToCache(comp);
            }
        }
        
        private void AddToCache(Component comp)
        {
            if (comp == null)
            {
                return;
            }

            Type concreteType = comp.GetType();
            
            AddEntryToPolymorphicCache(concreteType, comp);
            
            Type currentBaseType = concreteType.BaseType;
            while (currentBaseType != null && currentBaseType != typeof(UnityEngine.Object) &&
                   currentBaseType != typeof(Component))
            {
                AddEntryToPolymorphicCache(currentBaseType, comp);
                currentBaseType = currentBaseType.BaseType;
            }
            
            foreach (Type ifaceType in concreteType.GetInterfaces())
            {
                AddEntryToPolymorphicCache(ifaceType, comp);
            }
        }
        
        private void RemoveFromCache(Component comp)
        {
            if (comp == null)
            {
                return;
            }

            Type concreteType = comp.GetType();
            
            RemoveEntryFromPolymorphicCache(concreteType, comp);
            
            Type currentBaseType = concreteType.BaseType;
            while (currentBaseType != null && currentBaseType != typeof(UnityEngine.Object) &&
                   currentBaseType != typeof(Component))
            {
                RemoveEntryFromPolymorphicCache(currentBaseType, comp);
                currentBaseType = currentBaseType.BaseType;
            }
            
            foreach (Type ifaceType in concreteType.GetInterfaces())
            {
                RemoveEntryFromPolymorphicCache(ifaceType, comp);
            }
        }


        private void AddEntryToPolymorphicCache(Type type, Component comp)
        {
            if (!_polymorphicCache.TryGetValue(type, out var list))
            {
                list = new List<Component>();
                _polymorphicCache[type] = list;
            }

            if (!list.Contains(comp)) 
            {
                list.Add(comp);
            }
        }


        private void RemoveEntryFromPolymorphicCache(Type type, Component comp)
        {
            if (_polymorphicCache.TryGetValue(type, out var list))
            {
                list.Remove(comp);
                if (list.Count == 0)
                {
                    _polymorphicCache.Remove(type);
                }
            }
        }
        
        private void ClearAndAddComponents(IEnumerable<Component> componentsToAdd)
        {
            _containedComponents.Clear();
            foreach (var comp in componentsToAdd)
            {
                if (comp != null && !_containedComponents.Contains(comp))
                {
                    _containedComponents.Add(comp);
                }
            }

            MarkCacheDirty();
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this); 
#endif
        }
    }
}