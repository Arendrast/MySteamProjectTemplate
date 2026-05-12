using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Tools
{
    public static class VContainerTools
    {
        public static async UniTask CustomBuildAsync(this LifetimeScope scope)
        {
            await scope.BuildAsync();
        }
        
        public static void RegisterInstances(this IContainerBuilder containerBuilder, params object[] components)
        {
            for (var i = 0; i < components.Length; i++)
            {
                var instance = components[i];

                if (instance == null)
                {
                    Debug.LogError($"Component under {i} index is null");
                    continue;
                }
                
                containerBuilder.RegisterInstance(instance).As(instance.GetType());
            }
        }

        public static void RegisterMany(this IContainerBuilder builder, Lifetime lifeTime, params Type[][] types)
        {
            foreach (var serviceType in types.SelectMany(services => services))
                builder.Register(serviceType, lifeTime, true);
        }

        public static async UniTask RegisterAllInheritorsAsync<T>(this IContainerBuilder builder,
            Lifetime lifeTime) where T : class
        {
            var types = (await ReflectionTools.GetAllInheritorsTypesAsync<T>(false)).ToArray();
            builder.RegisterMany(lifeTime, types);
        }

        public static void Register(this IContainerBuilder builder, Type serviceType, Lifetime lifetime,
            bool asSelfAndAsImplementedInterfaces)
        {
            var registrationBuilder = builder.Register(serviceType, lifetime);

            if (asSelfAndAsImplementedInterfaces)
                registrationBuilder.AsSelf().AsImplementedInterfaces();
        }

        public static RegistrationBuilder WithParameters<T>(this RegistrationBuilder builder, Dictionary<string, T> objects)
        {
            foreach (var (key, value) in objects)
            {
                builder.WithParameter(key, value);
            }
            
            return builder;
        }
    }
}