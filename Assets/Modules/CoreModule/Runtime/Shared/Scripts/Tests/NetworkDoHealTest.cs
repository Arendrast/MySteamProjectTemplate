using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using FluentAssertions;
using Modules.EntityModule.Runtime.Shared.Scripts.Heal;
using Modules.PlayerModule.Runtime.Shared.Scripts.ClientPlayer;
using ParrelSync;
using UnityEngine.TestTools;

namespace Modules.CoreModule.Runtime.Shared.Scripts.Tests
{
    public class NetworkDoHealTest
    {
#if PARREL_SYNC_TESTS
        [UnityTest]
        public IEnumerator WhenDo1HPHealToNotOwnerPlayer_AndNoPingAndHPIsFull_ThenHPShouldBeFullPlus1()
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
            
            HealReceiverModel healReceiver = notOwnerPlayer.EntityComponents.HealReceiverModel;
            
            var startHealthPoints = healReceiver.HealthPoints;
            
            HealDealerModel healDealer = ownerPlayer.ClientComponents.EntityComponents.HealDealerModel;

            // Act.
            healDealer.DoHeal(healReceiver, new DoHealData(1, HealOrigin.None, healReceiver.MaxHealthPoints + 1));
            yield return UniTask.Delay(TimeSpan.FromSeconds(1), DelayType.Realtime).ToCoroutine(); // wait for player can react
            
            // Assert.
            healReceiver.HealthPoints.Should().Be(startHealthPoints + 1);
        }
        
        [UnityTest]
        public IEnumerator WhenReceive1HPHealFromHorst_AndNoPingAndHPIsFull_ThenHPShouldBeFullPlus1()
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
            
            HealReceiverModel healReceiver = ownerPlayer.ClientComponents.EntityComponents.HealReceiverModel;

            // Act.
            
            // Assert.
            
            healReceiver.HealthPoints.Should().Be(healReceiver.MaxHealthPoints + 1);
        }
#endif
    }
}