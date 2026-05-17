using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using FluentAssertions;
using Modules.EntityModule.Runtime.Shared.Scripts.Damage;
using Modules.PlayerModule.Runtime.Shared.Scripts.ClientPlayer;
using ParrelSync;
using UnityEngine.TestTools;

namespace Modules.AppModule.Runtime.Shared.Scripts.Tests
{
    public class NetworkDoDamageTest
    {
#if PARREL_SYNC_TESTS
        [UnityTest]
        public IEnumerator WhenDo1HPDamageToNotOwnerPlayer_AndNoPingAndHPIsFull_ThenHPShouldBeFullMinus1()
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

            var ownerPlayer = Setup.OwnerPlayer();
            
            DamageReceiverModel damageReceiver = notOwnerPlayer.EntityComponents.DamageReceiverModel;
            
            var startHealthPoints = damageReceiver.HealthPoints;
            
            DamageDealerModel damageDealer = ownerPlayer.ClientComponents.EntityComponents.DamageDealerModel;

            // Act.
            damageDealer.DoDamage(damageReceiver, new DoDamageData(1, DamageOrigin.None));
            yield return UniTask.Delay(TimeSpan.FromSeconds(1), DelayType.Realtime).ToCoroutine(); // wait for player can react
            
            // Assert.
            damageReceiver.HealthPoints.Should().Be(startHealthPoints - 1);
        }
        
        [UnityTest]
        public IEnumerator WhenReceive1HPDamageFromHost_AndNoPingAndHPIsFull_ThenHPShouldBeFullMinus1()
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
            
            DamageReceiverModel damageReceiver = ownerPlayer.ClientComponents.EntityComponents.DamageReceiverModel;

            // Act.
            
            // Assert.
            
            damageReceiver.HealthPoints.Should().Be(damageReceiver.MaxHealthPoints - 1);
        }

        [UnityTest]
        public IEnumerator WhenDo1HPDamageToOwnerPlayer_AndNoPingAndHPIs1_ThenHPShouldBe0()
        {
            // Arrange.
            foreach (var step in Setup.RestartPlayMode())
                yield return step;

            yield return Setup.MatchGameState(true);

            var ownerPlayer = Setup.OwnerPlayer();
            ownerPlayer.ClientComponents.EntityComponents.HealthModel.TrySetHealthPoints(1, 0);
            DamageReceiverModel damageReceiver = ownerPlayer.ClientComponents.EntityComponents.DamageReceiverModel;
            DamageDealerModel damageDealer = ownerPlayer.ClientComponents.EntityComponents.DamageDealerModel;

            // Act.
            damageDealer.DoDamage(damageReceiver, new DoDamageData(1, DamageOrigin.None));

            // Assert.
            damageReceiver.HealthPoints.Should().Be(0);
        }
#endif
    }
}