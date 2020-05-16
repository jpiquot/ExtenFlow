using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

using FluentAssertions;

using Moq;

using Xunit;

namespace ExtenFlow.Actors.Tests
{
    public class ActorBaseTest
    {
        private const string _stateName = "FakeBase";

        [Fact]
        public async Task ActorBaseGetState_ExpectGetStateAsync()
        {
            var stateManager = new Mock<IActorStateManager>();
            var state = new FakeState { FakeGuid = Guid.NewGuid(), FakeString = "hello world", FakeInt = 2000 };
            stateManager.Setup(manager => manager.GetStateAsync<FakeState>(_stateName, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(state))
                .Verifiable();
            // stateManager.Setup(manager => manager.SetStateAsync("FakeBase", state, It.IsAny<CancellationToken>())).Verifiable();
            FakeBaseActor testDemoActor = await CreateActor(stateManager.Object, state.FakeGuid);

            FakeState result = (FakeState)await testDemoActor.GetStateValue();
            result.Should().NotBeNull();
            result.FakeInt.Should().Be(state.FakeInt);
            result.FakeGuid.Should().Be(state.FakeGuid);
            result.FakeString.Should().Be(state.FakeString);

            stateManager.VerifyAll();
        }

        [Fact]
        public async Task ActorBaseIsInitialized_ExpectTrue()
        {
            var stateManager = new Mock<IActorStateManager>();
            var state = new FakeState { FakeGuid = Guid.NewGuid(), FakeString = "hello world", FakeInt = 2000 };
            stateManager.Setup(manager => manager.GetStateAsync<FakeState>(_stateName, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(state))
                .Verifiable();
            // stateManager.Setup(manager => manager.SetStateAsync("FakeBase", state, It.IsAny<CancellationToken>())).Verifiable();
            FakeBaseActor testDemoActor = await CreateActor(stateManager.Object, state.FakeGuid);

            bool result = await testDemoActor.IsInitialized();
            result.Should().BeTrue();

            stateManager.VerifyAll();
        }

        private async Task<FakeBaseActor> CreateActor(IActorStateManager actorStateManager, Guid id)
        {
            var actorTypeInformation = ActorTypeInformation.Get(typeof(FakeBaseActor));
            FakeBaseActor actorFactory(ActorService service, ActorId id) =>
                new FakeBaseActor(service, id, actorStateManager);
            var actorService = new ActorService(actorTypeInformation, actorFactory);
            FakeBaseActor actor = actorFactory(actorService, new ActorId(id.ToString()));
            MethodInfo OnActivate = actor.GetType().GetMethod("OnActivateAsync", BindingFlags.NonPublic | BindingFlags.Instance);
            await (Task)OnActivate.Invoke(actor, Array.Empty<object>());
            return actor;
        }
    }
}