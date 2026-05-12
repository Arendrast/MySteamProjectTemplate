using Cysharp.Threading.Tasks;
using Modules.SharedModule.Runtime.Shared.Scripts.Configs;
using Modules.SharedModule.Runtime.Shared.Scripts.Observers;
using Modules.SharedModule.Runtime.Shared.Scripts.Observers.Overlap;
using Modules.SharedModule.Runtime.Shared.Scripts.PhysicsPart;
using Modules.SharedModule.Runtime.Shared.Scripts.QoL;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Push
{
    public class CharacterControllerPushablesFactory : IMatchSharedFactory
    {
        private readonly PushablesRepository _explodableses;
        private readonly ConfigsProviderService _configsProviderService;

        public CharacterControllerPushablesFactory(
            PushablesRepository explodableses,
            ConfigsProviderService configsProviderService)
        {
            _explodableses = explodableses;
            _configsProviderService = configsProviderService;
        }

        public async UniTask<CharacterControllerPushHandlerController> TryCreateCharacterControllerPushHandlerAsync(
            ExplodableSerializableComponents explodableSerializableComponents,
            DataContainer<CharacterControllerPushHandlerModel> explosionModelContainer,
            bool shouldDisableCapsuleOverlapObserverWhenIsInactive)
        {
            if (_explodableses.ValueByKey.TryGetValue(explodableSerializableComponents, out var explodable) ||
                !explodableSerializableComponents.TryGetComponent<ManyInvokableOneFrameCharacterController>(out var characterController))
                return null;

            var config = await _configsProviderService.GetConfigAsync<CharacterControllerExplodablesConfig>();

            explosionModelContainer.Data = new CharacterControllerPushHandlerModel();
            
            var handler = new CharacterControllerPushHandlerController(
                characterController,
                explodableSerializableComponents.LocalMass,
                config.TimeMultiplier, config.MinimumThrowTime, config.SpeedCurve,
                explodableSerializableComponents.GetOrAddComponent<CapsuleOverlapObserver>(), explosionModelContainer.Data,
                shouldDisableCapsuleOverlapObserverWhenIsInactive);

            _explodableses.Add(explodableSerializableComponents, handler);

            explodableSerializableComponents.GetOrAddComponent<DisableObserver>().Disabled += Dispose;

            return handler;

            void Dispose()
            {
                _explodableses.RemoveByKey(explodableSerializableComponents);
            }
        }
    }
}