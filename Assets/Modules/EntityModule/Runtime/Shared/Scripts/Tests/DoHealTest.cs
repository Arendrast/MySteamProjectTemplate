using FluentAssertions;
using Modules.EntityModule.Runtime.Shared.Scripts.Damage;
using Modules.EntityModule.Runtime.Shared.Scripts.Entity;
using Modules.EntityModule.Runtime.Shared.Scripts.Heal;
using NUnit.Framework;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Tests
{
    public class DoHealTest
    {
        [Test]
        public void WhenDo1HPHeal_AndHPIs1_ThenHPBecome2()
        {
            // Arrange.
            var startHealthPoints = 1;
            var maxHealPoints = startHealthPoints + 1;
            var healthModel = new HealthModel(maxHealPoints, startHealthPoints);
            var healReceiverModel = new HealReceiverModel(healthModel);
            var healDealerModel = new HealDealerModel(0);
            
            // Act.
            healDealerModel.DoHeal(healReceiverModel, new DoHealData(1, HealOrigin.None));

            // Assert.
            healthModel.HealthPoints.Should().Be(startHealthPoints + 1);
        }   
    }
}