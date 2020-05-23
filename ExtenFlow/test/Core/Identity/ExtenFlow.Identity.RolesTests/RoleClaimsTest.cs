using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

using ExtenFlow.Actors;
using ExtenFlow.EventStorage;
using ExtenFlow.Identity.Roles.Actors;
using ExtenFlow.Identity.Roles.Commands;
using ExtenFlow.Domain.Dispatcher;

using FluentAssertions;

using Moq;

using Xunit;

namespace ExtenFlow.Identity.RolesTests
{
    public class RoleClaimsActorTest
    {
        private const string _stateName = "RoleClaims";

        [Fact]
        public async Task RoleClaimsActorAdd_ExpectGetStateAsync()
        {
            var stateManager = new Mock<IActorStateManager>();
            var state = new RoleClaimsEntity("testrole", new Dictionary<string, HashSet<string>>
            {
                { "type 1", new HashSet<string>{"type 1 value" } }
            });
            stateManager.Setup(manager => manager.GetStateAsync<RoleClaimsEntity>(_stateName, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(state))
                .Verifiable();
            RoleClaimsActor testDemoActor = await CreateActor(stateManager.Object, Guid.NewGuid().ToString());

            object result = await testDemoActor.GetStateValue();
            result.Should().NotBeNull();
            result.Should().BeOfType<RoleState>();
            var value = (RoleState)result;
            value.Should().NotBeNull();
            value.Name.Should().Be(state.Name);
            value.NormalizedName.Should().Be(state.NormalizedName);
            value.ConcurrencyStamp.Should().Be(state.ConcurrencyStamp);
            stateManager.VerifyAll();
        }

        [Fact]
        public async Task RoleClaimsActorAdd_ExpectsValue()
        {
            var stateManager = new Mock<IActorStateManager>();
            var state1 = new RoleState("new role", "NEWROLE", null);
            stateManager.Setup(manager => manager
                .SetStateAsync(
                    _stateName,
                    It.Is<RoleState>(o =>
                        o.Name == state1.Name &&
                        o.NormalizedName == state1.NormalizedName &&
                        !string.IsNullOrWhiteSpace(o.ConcurrencyStamp)),
                    It.IsAny<CancellationToken>()))
                .Verifiable();
            string id = Guid.NewGuid().ToString();
            RoleClaimsActor testDemoActor = await CreateActor(stateManager.Object, id);

            await testDemoActor.Tell(new AddNewRole(id, state1.Name, state1.NormalizedName, "user"));
            stateManager.VerifyAll();
        }

        [Fact]
        public async Task RoleClaimsActorRemove_ExpectsRemoveState()
        {
            var stateManager = new Mock<IActorStateManager>();
            var oldState = new RoleState("old role", "OLDROLE", Guid.NewGuid().ToString());
            stateManager.Setup(manager => manager
                .GetStateAsync<RoleState>(_stateName, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(oldState))
                .Verifiable();
            stateManager.Setup(manager => manager
                .RemoveStateAsync(_stateName, It.IsAny<CancellationToken>()))
                .Verifiable();
            string id = Guid.NewGuid().ToString();
            RoleClaimsActor testDemoActor = await CreateActor(stateManager.Object, id);

            await testDemoActor.Tell(new RemoveRole(id, oldState.ConcurrencyStamp, "user"));
            stateManager.VerifyAll();
        }

        [Fact]
        public async Task RoleClaimsActorRename_ExpectsNewValue()
        {
            var stateManager = new Mock<IActorStateManager>();
            var oldState = new RoleState("old role", "OLDROLE", Guid.NewGuid().ToString());
            var newState = new RoleState("new role", "NEWROLE", null);
            stateManager.Setup(manager => manager.GetStateAsync<RoleState>(_stateName, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(oldState))
                .Verifiable();
            stateManager.Setup(manager => manager
                .SetStateAsync(
                    _stateName,
                    It.Is<RoleState>(o =>
                        o.Name == newState.Name &&
                        o.NormalizedName == newState.NormalizedName &&
                        string.IsNullOrWhiteSpace(o.ConcurrencyStamp) &&
                        o.ConcurrencyStamp != oldState.ConcurrencyStamp),
                    It.IsAny<CancellationToken>()))
                .Verifiable();
            string id = Guid.NewGuid().ToString();
            RoleClaimsActor testDemoActor = await CreateActor(stateManager.Object, id);

            await testDemoActor.Tell(new RenameRole(id, newState.Name, newState.NormalizedName, oldState.ConcurrencyStamp, "user"));
            stateManager.VerifyAll();
        }

        private async Task<RoleClaimsActor> CreateActor(IActorStateManager actorStateManager, string id)
        {
            var uniqueIndexActor = new Mock<IUniqueIndexActor>();
            var eventBus = new Mock<IEventBus>();
            var eventStore = new Mock<IEventStore>();
            var actorTypeInformation = ActorTypeInformation.Get(typeof(RoleClaimsActor));
            RoleClaimsActor actorFactory(ActorService service, ActorId id) =>
                new RoleClaimsActor(service, id, eventBus.Object, eventStore.Object, actorStateManager);
            var actorService = new ActorService(actorTypeInformation, actorFactory);
            RoleClaimsActor actor = actorFactory(actorService, new ActorId(id));
            MethodInfo OnActivate = actor.GetType().GetMethod("OnActivateAsync", BindingFlags.NonPublic | BindingFlags.Instance);
            await (Task)OnActivate.Invoke(actor, Array.Empty<object>());
            return actor;
        }
    }
}