using System;
using Modules.SharedModule.Runtime.Shared.Scripts.Observers;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Modules.SharedModule.Runtime.Client.Scripts.Localization
{
    public static class SubscribingTools
    {
        public static void SubscribeToLocalizationAndSubscribeOnDestroy(this GameObject gameObject,
            Action<Locale> actions)
        {
            if (!gameObject)
                return;

            LocalizationSettings.SelectedLocaleChanged += actions;
            
            gameObject.GetOrAddComponent<DestroyObserver>().Destroyed += () => LocalizationSettings.SelectedLocaleChanged -= actions;
        }
    }
}