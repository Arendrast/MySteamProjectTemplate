using FluentAssertions;
using Modules.EntityModule.Runtime.Shared.Scripts.Damage;
using Modules.EntityModule.Runtime.Shared.Scripts.Entity;
using NUnit.Framework;
using UnityEngine;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Tests
{
    public class DoDamageTest
    {
        [Test]
        public void WhenDo1Damage_AndHPIs1_ThenHPBecome0()
        {
            // Arrange.
            var startHealthPoints = 1;
            var healthModel = new HealthModel(startHealthPoints);
            var damageReceiverModel = new DamageReceiverModel(0, healthModel);
            var damageDealerModel = new DamageDealerModel(0);
            
            // Act.
            damageDealerModel.DoDamage(damageReceiverModel, new DoDamageData(1, DamageOrigin.None));

            // Assert.
            healthModel.HealthPoints.Should().Be(startHealthPoints - 1);
        }
    }
}
