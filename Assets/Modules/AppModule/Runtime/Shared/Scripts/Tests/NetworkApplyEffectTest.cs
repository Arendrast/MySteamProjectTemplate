using System;
using System.Collections;
using System.Linq;
using Cysharp.Threading.Tasks;
using FluentAssertions;
using Modules.EntityModule.Runtime.Shared.Scripts.Effects;
using Modules.EntityModule.Runtime.Shared.Scripts.Effects.Effectable.Logic;
using Modules.PlayerModule.Runtime.Shared.Scripts.ClientPlayer;
using ParrelSync;
using UnityEngine.TestTools;

namespace Modules.AppModule.Runtime.Shared.Scripts.Tests
{
    public class NetworkApplyEffectTest
    {
#if PARREL_SYNC_TESTS
        [UnityTest]
        public IEnumerator WhenApplyNoneEffectToNotOwnerPlayer_AndNoEffects_ThenPlayerFirstEffectShouldBeNone()
        {
            if (ClonesManager.IsClone())
            {
                yield break;
            }

            // Arrange.
            foreach (var step in Setup.RestartPlayMode())
                yield return step;

            yield return Setup.MatchGameState(true);

            ClientPlayerComponents notOwnerPlayer = null;
            yield return Setup.WaitForConnectAnotherPlayerAndInvokeAction(components => notOwnerPlayer = components);

            EffectsReceiverModel effectsReceiver = notOwnerPlayer.EntityComponents.EffectsReceiverModel;

            EffectApplierController effectApplier = Setup.EffectApplierController();

            // Act.

            if (!effectApplier.TryApplyEffect(notOwnerPlayer.SerializableComponents
                    .GetComponent<EffectableSerializableComponents>()))
            {
                throw new Exception("Cant apply effect");
            }
            
            yield return
                UniTask.Delay(TimeSpan.FromSeconds(1f), DelayType.Realtime).ToCoroutine(); // wait for player can react

            // Assert.
            effectsReceiver.ActiveEffects.First().Should().Be(EffectType.None);
        }

        [UnityTest]
        public IEnumerator WhenReceiveNoneEffectFromHost_AndNoPingAndNoEffects_ThenFirstEffectShouldBeNone()
        {
            if (!ClonesManager.IsClone())
            {
                yield break;
            }

            // Arrange.
            foreach (var step in Setup.RestartPlayMode())
                yield return step;

            yield return Setup.MatchGameState(false);

            var ownerPlayer = Setup.OwnerPlayer();

            EffectsReceiverModel effectsReceiver = ownerPlayer.ClientComponents.EntityComponents.EffectsReceiverModel;

            // Act.

            // Assert.

            effectsReceiver.ActiveEffects.First().Should().Be(EffectType.None);
        }
#endif
    }
}