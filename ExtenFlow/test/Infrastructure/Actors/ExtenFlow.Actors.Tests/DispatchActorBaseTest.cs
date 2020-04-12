using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

using ExtenFlow.Messages;
using ExtenFlow.Messages.Dispatcher;

using FluentAssertions;

using Moq;

using Xunit;

using static FluentAssertions.FluentActions;

namespace ExtenFlow.Actors.Tests
{
    public class DispatchActorBaseTest
    {
        private const string _stateName = "FakeDispatch";

        [Fact]
        public async Task CreateCommand_CheckValue()
        {
            var stateManager = new Mock<IActorStateManager>();
            var messageQueue = new Mock<IMessageQueue>();
            var state = new FakeState { FakeGuid = Guid.NewGuid(), FakeString = "hello world", FakeInt = 2000 };
            stateManager.Setup(manager => manager.SetStateAsync(_stateName, state, It.IsAny<CancellationToken>())).Verifiable();
            FakeDispatchActor testDemoActor = await CreateActor(stateManager.Object, messageQueue.Object, Guid.NewGuid());

            await testDemoActor.Tell(new CreateFakeDispatch(state.FakeGuid, state.FakeInt, state.FakeString));
            stateManager.VerifyAll();
        }

        [Fact]
        public async Task CreateCommand_ExpectCreatedEvent()
        {
            var stateManager = new Mock<IActorStateManager>();
            var messageQueue = new Mock<IMessageQueue>();
            var messageId = Guid.NewGuid();
            var state = new FakeState { FakeGuid = Guid.NewGuid(), FakeString = "hello world", FakeInt = 2000 };
            var events = new List<IEvent>(new[] { new FakeDispatchCreated(state.FakeGuid, state.FakeInt, state.FakeString) });
            messageQueue
                .Setup(messageQueue => messageQueue.Send(events))
                .Returns(Task.FromResult(messageId))
                .Verifiable();
            messageQueue
                .Setup(messageQueue => messageQueue.ConfirmSend(messageId))
                .Verifiable();
            FakeDispatchActor testDemoActor = await CreateActor(stateManager.Object, messageQueue.Object, Guid.NewGuid());

            await testDemoActor.Tell(new CreateFakeDispatch(state.FakeGuid, state.FakeInt, state.FakeString));
            stateManager.VerifyAll();
        }

        [Fact]
        public async Task GetFakeInt_CheckValue()
        {
            var stateManager = new Mock<IActorStateManager>();
            var messageQueue = new Mock<IMessageQueue>();
            var state = new FakeState { FakeGuid = Guid.NewGuid(), FakeString = "hello world", FakeInt = 2000 };
            stateManager.Setup(manager => manager.GetStateAsync<FakeState>(_stateName, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(state))
                .Verifiable();
            // stateManager.Setup(manager => manager.SetStateAsync("FakeBase", state, It.IsAny<CancellationToken>())).Verifiable();
            FakeDispatchActor testDemoActor = await CreateActor(stateManager.Object, messageQueue.Object, state.FakeGuid);

            int result = await testDemoActor.Ask(new GetFakeDispatchInt(state.FakeGuid));
            result.Should().Be(state.FakeInt);

            stateManager.VerifyAll();
        }

        [Fact]
        public async Task UnkownCommand_ExpectNotSupportedException()
        {
            var stateManager = new Mock<IActorStateManager>();
            var messageQueue = new Mock<IMessageQueue>();
            FakeDispatchActor testDemoActor = await CreateActor(stateManager.Object, messageQueue.Object, Guid.NewGuid());

            await Invoking(async () => await testDemoActor.Tell(new FakeDispatchUnknownCommand()))
                            .Should()
                            .ThrowAsync<NotSupportedException>();
        }

        [Fact]
        public async Task UnkownEvent_ExpectIgnored()
        {
            var stateManager = new Mock<IActorStateManager>();
            var messageQueue = new Mock<IMessageQueue>();
            FakeDispatchActor testDemoActor = await CreateActor(stateManager.Object, messageQueue.Object, Guid.NewGuid());

            await testDemoActor.Notify(new FakeDispatchUnknownEvent());
        }

        [Fact]
        public async Task UnkownMessage_ExpectIgnored()
        {
            var stateManager = new Mock<IActorStateManager>();
            var messageQueue = new Mock<IMessageQueue>();
            FakeDispatchActor testDemoActor = await CreateActor(stateManager.Object, messageQueue.Object, Guid.NewGuid());

            await testDemoActor.Notify(new FakeDispatchUnknownMessage());
        }

        [Fact]
        public async Task UnkownQuery_ExpectNotSupportedException()
        {
            var stateManager = new Mock<IActorStateManager>();
            var messageQueue = new Mock<IMessageQueue>();
            FakeDispatchActor testDemoActor = await CreateActor(stateManager.Object, messageQueue.Object, Guid.NewGuid());

            await Invoking(async () => await testDemoActor.Ask(new FakeDispatchUnknownQuery()))
                            .Should()
                            .ThrowAsync<NotSupportedException>();
        }

        private async Task<FakeDispatchActor> CreateActor(IActorStateManager actorStateManager, IMessageQueue messageQueue, Guid id)
        {
            var actorTypeInformation = ActorTypeInformation.Get(typeof(FakeDispatchActor));
            FakeDispatchActor actorFactory(ActorService service, ActorId id) =>
                new FakeDispatchActor(service, id, messageQueue, actorStateManager);
            var actorService = new ActorService(actorTypeInformation, actorFactory);
            FakeDispatchActor actor = actorFactory(actorService, new ActorId(id.ToString()));
            MethodInfo OnActivate = actor.GetType().GetMethod("OnActivateAsync", BindingFlags.NonPublic | BindingFlags.Instance);
            await (Task)OnActivate.Invoke(actor, Array.Empty<object>());
            return actor;
        }
    }
}