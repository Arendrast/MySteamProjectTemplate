using System.Threading.Tasks;
using FluentAssertions;
using Modules.CoreModule.Runtime.Shared.Scripts.Infrastructure;
using Modules.EntityModule.Runtime.Shared.Scripts.Damage;
using Modules.PlayerModule.Runtime.Shared.Scripts.DependencyInjection;
using Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace Modules.CoreModule.Runtime.Shared.Scripts.Tests
{
    public class NetworkDoDamageTest
    {
        [Test]
        public async Task WhenDo1HPDamageToOwnerPlayer_AndNoPingAndHPIs1_ThenHPShouldBe0()
        {
            // Arrange.
            await Setup.MatchGameState(true);

            var playerFactory = LifetimeScope.Find<MatchSharedServicesScope>().Container.Resolve<OwnerPlayerFactory>();
            playerFactory.OwnerPlayerComponents.ClientComponents.EntityComponents.HealthModel.TrySetHealthPoints(1, 0);
            DamageReceiverModel damageReceiver = playerFactory.OwnerPlayerComponents.ClientComponents.EntityComponents.DamageReceiverModel;
            DamageDealerModel damageDealer = playerFactory.OwnerPlayerComponents.ClientComponents.EntityComponents.DamageDealerModel;
            
            // Act.
            damageDealer.DoDamage(damageReceiver, new DoDamageData(1, DamageOrigin.None));

            // Assert.
            damageReceiver.HealthPoints.Should().Be(0);
        }
    }
}
