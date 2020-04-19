using System;
using System.Collections.Generic;
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
    public class UniqueIndexActorTest
    {
        private const string _stateName = "FakeUniqueIndexActor";

        [Fact]
        public async Task UniqueIndexActorAdd_ExpectGetStateAsync()
        {
            var stateManager = new Mock<IActorStateManager>();
            var state1 = new Dictionary<string, string> { { "Test1", Guid.NewGuid().ToString() }, { "Test2", Guid.NewGuid().ToString() }, { "Test3", Guid.NewGuid().ToString() }, { "Test4", Guid.NewGuid().ToString() }, { "Test5", Guid.NewGuid().ToString() }, { "Test6", Guid.NewGuid().ToString() }, { "Test7", Guid.NewGuid().ToString() } };
            stateManager.Setup(manager => manager.GetStateAsync<Dictionary<string, string>>(_stateName, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(state1))
                .Verifiable();
            UniqueIndexActor testDemoActor = await CreateActor(stateManager.Object, "Test UniqueIndexActor");

            object result = await testDemoActor.GetStateValue();
            result.Should().NotBeNull();
            result.Should().BeOfType<Dictionary<string, string>>();
            var ids = (Dictionary<string, string>)result;
            ids.Should().HaveCount(state1.Count);
            ids.Should().BeEquivalentTo(state1);

            stateManager.VerifyAll();
        }

        [Fact]
        public async Task UniqueIndexActorAdd_ExpectsNewStateWithNewItems()
        {
            var stateManager = new Mock<IActorStateManager>();
            var state1 = new Dictionary<string, string> { { "Test1", Guid.NewGuid().ToString() }, { "Test2", Guid.NewGuid().ToString() }, { "Test3", Guid.NewGuid().ToString() }, { "Test4", Guid.NewGuid().ToString() }, { "Test5", Guid.NewGuid().ToString() }, { "Test6", Guid.NewGuid().ToString() }, { "Test7", Guid.NewGuid().ToString() } };
            var state2 = new Dictionary<string, string> { { "Test8", Guid.NewGuid().ToString() }, { "Test9", Guid.NewGuid().ToString() }, { "Test10", Guid.NewGuid().ToString() } };
            stateManager.Setup(manager => manager.GetStateAsync<Dictionary<string, string>>(_stateName, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(state1))
                .Verifiable();
            stateManager.Setup(manager => manager.SetStateAsync(_stateName, state1, It.IsAny<CancellationToken>()))
                .Verifiable();
            UniqueIndexActor testDemoActor = await CreateActor(stateManager.Object, "Test UniqueIndexActor");

            await testDemoActor.Add("Test8", state2["Test8"]);
            await testDemoActor.Add("Test9", state2["Test9"]);
            await testDemoActor.Add("Test10", state2["Test10"]);
            state1.Should().HaveCount(10);
            state1.Should().Contain(state2);

            stateManager.VerifyAll();
        }

        [Fact]
        public async Task UniqueIndexActorRemove_ExpectsNewStateWithoutRemovedItems()
        {
            var stateManager = new Mock<IActorStateManager>();
            var list = new Dictionary<string, string> { { "Test1", Guid.NewGuid().ToString() }, { "Test2", Guid.NewGuid().ToString() }, { "Test3", Guid.NewGuid().ToString() }, { "Test4", Guid.NewGuid().ToString() }, { "Test5", Guid.NewGuid().ToString() }, { "Test6", Guid.NewGuid().ToString() }, { "Test7", Guid.NewGuid().ToString() } };
            var state = new Dictionary<string, string>(list);
            stateManager.Setup(manager => manager.GetStateAsync<Dictionary<string, string>>(_stateName, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(state))
                .Verifiable();
            stateManager.Setup(manager => manager.SetStateAsync(_stateName, state, It.IsAny<CancellationToken>()))
                .Verifiable();
            UniqueIndexActor testDemoActor = await CreateActor(stateManager.Object, "Test UniqueIndexActor");

            await testDemoActor.Remove("Test1");
            await testDemoActor.Remove("Test5");
            await testDemoActor.Remove("Test7");
            list.Remove("Test1");
            list.Remove("Test5");
            list.Remove("Test7");
            state.Should().HaveCount(4);
            state.Should().Contain(list);

            stateManager.VerifyAll();
        }

        private async Task<UniqueIndexActor> CreateActor(IActorStateManager actorStateManager, string id)
        {
            var actorTypeInformation = ActorTypeInformation.Get(typeof(UniqueIndexActor));
            UniqueIndexActor actorFactory(ActorService service, ActorId id) =>
                new UniqueIndexActor(service, id, actorStateManager);
            var actorService = new ActorService(actorTypeInformation, actorFactory);
            UniqueIndexActor actor = actorFactory(actorService, new ActorId(id));
            MethodInfo OnActivate = actor.GetType().GetMethod("OnActivateAsync", BindingFlags.NonPublic | BindingFlags.Instance);
            await (Task)OnActivate.Invoke(actor, Array.Empty<object>());
            return actor;
        }
    }
}