using System;
using System.Collections;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Modules.CoreModule.Runtime.Shared.Scripts.Infrastructure;
using Modules.LevelModule.Runtime.Shared.Scripts;
using Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer;
using NUnit.Framework;
using Unity.Multiplayer.PlayMode;
using UnityEngine;
using UnityEngine.TestTools;
using VContainer;
using VContainer.Unity;

namespace Modules.CoreModule.Runtime.Shared.Scripts.Tests
{
    public class InfrastructureTests
    {
        [UnityTest]
        public IEnumerator WhenStartLevel1_ThenPlayerShouldNotBeNull()
        {
            // Arrange.
            foreach (var step in Setup.RestartPlayMode()) 
                yield return step;
            
            // Act.
            yield return Setup.MatchGameState(true);

            // Assert.
            var scope = LifetimeScope.Find<MatchSharedServicesScope>();
            scope.Container.Resolve<OwnerPlayerFactory>()
                .OwnerPlayerComponents.Should().NotBeNull();
        }

        [UnityTest]
        public IEnumerator WhenStartLevel1_ThenLevelZoneShouldNotBeNull()
        {
            // Arrange.
            foreach (var step in Setup.RestartPlayMode()) 
                yield return step;
            
            // Act.
            yield return Setup.MatchGameState(true);
            
            // Assert.
            var scope = LifetimeScope.Find<MatchSharedServicesScope>();
            scope.Container.Resolve<LevelZoneRepository>().PersistentObjectsLevelZoneSerializableComponents.Should()
                .NotBeNull();
        }
    }
}