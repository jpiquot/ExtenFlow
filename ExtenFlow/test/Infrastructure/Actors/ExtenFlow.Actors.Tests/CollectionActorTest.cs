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
    public class CollectionActorTest
    {
        private const string _stateName = "Collection";

        [Fact]
        public async Task CollectionActorAdd_ExpectGetStateAsync()
        {
            var stateManager = new Mock<IActorStateManager>();
            var state = new HashSet<string> { Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString() };
            stateManager.Setup(manager => manager.GetStateAsync<HashSet<string>>(_stateName, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(state))
                .Verifiable();
            CollectionActor testDemoActor = await CreateActor(stateManager.Object, "Test Collection");

            object result = await testDemoActor.GetStateValue();
            result.Should().NotBeNull();
            result.Should().BeOfType<HashSet<string>>();
            var ids = (HashSet<string>)result;
            ids.Should().HaveCount(state.Count);
            ids.Should().BeEquivalentTo(state);

            stateManager.VerifyAll();
        }

        [Fact]
        public async Task CollectionActorAdd_ExpectsNewStateWithNewItems()
        {
            var stateManager = new Mock<IActorStateManager>();
            var state1 = new HashSet<string> { Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString() };
            var state2 = new List<string> { Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString() };
            stateManager.Setup(manager => manager.GetStateAsync<HashSet<string>>(_stateName, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(state1))
                .Verifiable();
            stateManager.Setup(manager => manager.SetStateAsync(_stateName, state1, It.IsAny<CancellationToken>()))
                .Verifiable();
            CollectionActor testDemoActor = await CreateActor(stateManager.Object, "Test Collection");

            await testDemoActor.Add(state2[0]);
            await testDemoActor.Add(state2[1]);
            await testDemoActor.Add(state2[2]);
            state1.Should().HaveCount(8);
            state1.Should().Contain(state2);

            stateManager.VerifyAll();
        }

        [Fact]
        public async Task CollectionActorAll_ExpectsAllItems()
        {
            var stateManager = new Mock<IActorStateManager>();
            var list = new List<string> { Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString() };
            var state = new HashSet<string>(list);
            stateManager.Setup(manager => manager.GetStateAsync<HashSet<string>>(_stateName, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(state))
                .Verifiable();
            CollectionActor testDemoActor = await CreateActor(stateManager.Object, "Test Collection");

            IList<string> result = await testDemoActor.All();
            result.Should().HaveCount(list.Count);
            result.Should().Contain(state);

            stateManager.VerifyAll();
        }

        [Fact]
        public async Task CollectionActorExist_ExpectsFalse()
        {
            var stateManager = new Mock<IActorStateManager>();
            var list = new List<string> { Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString() };
            var state = new HashSet<string>(list);
            stateManager.Setup(manager => manager.GetStateAsync<HashSet<string>>(_stateName, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(state))
                .Verifiable();
            CollectionActor testDemoActor = await CreateActor(stateManager.Object, "Test Collection");

            bool result = await testDemoActor.Exist("this value does not exist");
            result.Should().BeFalse();

            stateManager.VerifyAll();
        }

        [Fact]
        public async Task CollectionActorExist_ExpectsTrue()
        {
            var stateManager = new Mock<IActorStateManager>();
            var list = new List<string> { Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString() };
            var state = new HashSet<string>(list);
            stateManager.Setup(manager => manager.GetStateAsync<HashSet<string>>(_stateName, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(state))
                .Verifiable();
            CollectionActor testDemoActor = await CreateActor(stateManager.Object, "Test Collection");

            bool result = await testDemoActor.Exist(list[3]);
            result.Should().BeTrue();

            stateManager.VerifyAll();
        }

        [Fact]
        public async Task CollectionActorRemove_ExpectsNewStateWithoutRemovedItems()
        {
            var stateManager = new Mock<IActorStateManager>();
            var list = new List<string> { Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString() };
            var state = new HashSet<string>(list);
            stateManager.Setup(manager => manager.GetStateAsync<HashSet<string>>(_stateName, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(state))
                .Verifiable();
            stateManager.Setup(manager => manager.SetStateAsync(_stateName, state, It.IsAny<CancellationToken>()))
                .Verifiable();
            CollectionActor testDemoActor = await CreateActor(stateManager.Object, "Test Collection");

            await testDemoActor.Remove(list[2]);
            await testDemoActor.Remove(list[0]);
            list.Remove(list[2]);
            list.Remove(list[0]);
            state.Should().HaveCount(3);
            state.Should().Contain(list);

            stateManager.VerifyAll();
        }

        private async Task<CollectionActor> CreateActor(IActorStateManager actorStateManager, string id)
        {
            var actorTypeInformation = ActorTypeInformation.Get(typeof(CollectionActor));
            CollectionActor actorFactory(ActorService service, ActorId id) =>
                new CollectionActor(service, id, actorStateManager);
            var actorService = new ActorService(actorTypeInformation, actorFactory);
            CollectionActor actor = actorFactory(actorService, new ActorId(id));
            MethodInfo OnActivate = actor.GetType().GetMethod("OnActivateAsync", BindingFlags.NonPublic | BindingFlags.Instance);
            await (Task)OnActivate.Invoke(actor, Array.Empty<object>());
            return actor;
        }
    }
}