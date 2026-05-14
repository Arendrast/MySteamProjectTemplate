using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Modules.CoreModule.Runtime.Shared.Scripts.Infrastructure;
using Modules.LevelModule.Runtime.Shared.Scripts;
using Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer;
using NUnit.Framework;
using VContainer;
using VContainer.Unity;

namespace Modules.CoreModule.Runtime.Shared.Scripts.Tests
{
    public class InfrastructureTests
    {
        [Test]
        public async Task WhenStartLevel1_ThenExceptionBeNull()
        {
            // Arrange.
            Exception exception = null;

            // Act.
            try
            {
                await Setup.MatchGameState(true);
            }
            catch (Exception e)
            {
                exception = e;
            }

            // Assert.
            exception.Should().BeNull();
        }

        [Test]
        public async Task WhenStartLevel1_ThenPlayerShouldNotBeNull()
        {
            // Arrange.

            // Act.
            await Setup.MatchGameState(true);

            // Assert.
            var scope = LifetimeScope.Find<MatchSharedServicesScope>();
            scope.Container.Resolve<OwnerPlayerFactory>()
                .OwnerPlayerComponents.Should().NotBeNull();
        }

        [Test]
        public async Task WhenStartLevel1_ThenLevelZoneShouldNotBeNull()
        {
            // Arrange.

            // Act.
            await Setup.MatchGameState(true);

            // Assert.
            var scope = LifetimeScope.Find<MatchSharedServicesScope>();
            scope.Container.Resolve<LevelZoneRepository>().PersistentObjectsLevelZoneSerializableComponents.Should()
                .NotBeNull();
        }
    }
}