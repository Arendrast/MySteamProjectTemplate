#if !DISABLESTEAMWORKS  && (STEAMWORKSNET || STEAM_LEGACY || STEAM_161 || STEAM_162)
using System;
using Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime.Data;
using UnityEngine;

namespace Heathen.SteamworksIntegration.Modules.HealthenSteamworksModule.Runtime
{
    [Serializable]
    public struct InputActionData
    {
        public readonly InputActionType Type => type;
        public readonly string Name => name;

        public InputActionData(string actionName, InputActionType actionType)
        {
            type = actionType;
            name = actionName;
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [SerializeField]
        private InputActionType type;
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [SerializeField]
        private string name;

        public readonly Steamworks.InputAnalogActionHandle_t AnalogHandle => Input.Client.GetAnalogActionHandle(name);
        public readonly Steamworks.InputDigitalActionHandle_t DigitalHandle => Input.Client.GetDigitalActionHandle(name);
        
        public readonly InputActionStateData GetActionData(Steamworks.InputHandle_t controller) => Input.Client.GetActionData(controller, name);
        public readonly InputActionStateData GetActionData() => Input.Client.GetActionData(name);
        public readonly Texture2D[] GetInputGlyphs(Steamworks.InputHandle_t controller, InputActionSetData set) => GetInputGlyphs(controller, set);
        public readonly Texture2D[] GetInputGlyphs(Steamworks.InputHandle_t controller, InputActionSetLayerData set) => GetInputGlyphs(controller, set.Data);
        public readonly Texture2D[] GetInputGlyphs(Steamworks.InputHandle_t controller, Steamworks.InputActionSetHandle_t set)
        {
            if (type == InputActionType.Analog)
            {
                var origins = Input.Client.GetAnalogActionOrigins(controller, set, AnalogHandle);

                var textArray = new Texture2D[origins.Length];
                for (int i = 0; i < origins.Length; i++)
                {
                    textArray[i] = Input.Client.GetGlyphActionOrigin(origins[i]);
                }

                return textArray;
            }
            else
            {
                var origins = Input.Client.GetDigitalActionOrigins(controller, set, DigitalHandle);

                var textArray = new Texture2D[origins.Length];
                for (int i = 0; i < origins.Length; i++)
                {
                    textArray[i] = Input.Client.GetGlyphActionOrigin(origins[i]);
                }

                return textArray;
            }
        }

        public readonly string[] GetInputNames(Steamworks.InputHandle_t controller, InputActionSetData set) => GetInputNames(controller, set);
        public readonly string[] GetInputNames(Steamworks.InputHandle_t controller, InputActionSetLayerData set) => GetInputNames(controller, set.Data);
        public readonly string[] GetInputNames(Steamworks.InputHandle_t controller, Steamworks.InputActionSetHandle_t set)
        {
            if (type == InputActionType.Analog)
            {
                var origins = Input.Client.GetAnalogActionOrigins(controller, set, AnalogHandle);

                var stringArray = new string[origins.Length];
                for (int i = 0; i < origins.Length; i++)
                {
                    stringArray[i] = Input.Client.GetStringForActionOrigin(origins[i]);
                }

                return stringArray;
            }
            else
            {
                var origins = Input.Client.GetDigitalActionOrigins(controller, set, DigitalHandle);

                var stringArray = new string[origins.Length];
                for (int i = 0; i < origins.Length; i++)
                {
                    stringArray[i] = Input.Client.GetStringForActionOrigin(origins[i]);
                }

                return stringArray;
            }
        }
    }
}
#endif