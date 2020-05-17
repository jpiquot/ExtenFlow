using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

using ExtenFlow.Domain;

using FluentAssertions;

using Moq;

using Xunit;

namespace ExtenFlow.EventStorage.DaprActors.Tests
{
    public class EventStoreStreamActorTest
    {
        private const string _stateName = "EventStoreStream";

        [Fact]
        public async Task EventStoreStreamActor_GetState_ExpectGetStateAsync()
        {
            var stateManager = new Mock<IActorStateManager>();
            FakeEvent[] events = new[] { new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent() };
            var state = new EventStoreStreamState();
            state.Append(events);
            stateManager.Setup(manager => manager.GetStateAsync<EventStoreStreamState>(_stateName, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(state))
                .Verifiable();
            EventStoreStreamActor testDemoActor = await CreateActor(stateManager.Object, EventStoreStreamActor.CreateStreamId(events[0].AggregateType, events[0].AggregateId));

            EventStoreStreamState result = (EventStoreStreamState)await testDemoActor.GetStateValue();
            result.Should().Be(state);

            stateManager.VerifyAll();
        }

        [Fact]
        public async Task EventStoreStreamActor_Read_ExpectGetStateAsync()
        {
            var stateManager = new Mock<IActorStateManager>();
            FakeEvent[] events = new[] { new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent() };
            var state = new EventStoreStreamState();
            state.Append(events);
            stateManager.Setup(manager => manager.GetStateAsync<EventStoreStreamState>(_stateName, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(state))
                .Verifiable();
            EventStoreStreamActor testDemoActor = await CreateActor(stateManager.Object, EventStoreStreamActor.CreateStreamId(events[0].AggregateType, events[0].AggregateId));

            IList<IEvent> result = await testDemoActor.Read();
            CheckValues(state.Read(), result);

            stateManager.VerifyAll();
        }

        [Fact]
        public async Task EventStoreStreamActor_ReadAfterFive_ExpectFiveAfterEvents()
        {
            var stateManager = new Mock<IActorStateManager>();
            FakeEvent[] testEvents1 = new[] { new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent() };
            FakeEvent[] testEvents2 = new[] { new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent() };
            var state = new EventStoreStreamState();
            state.Append(testEvents1);
            state.Append(testEvents2);
            stateManager.Setup(manager => manager.GetStateAsync<EventStoreStreamState>(_stateName, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(state))
                .Verifiable();

            EventStoreStreamActor testDemoActor = await CreateActor(stateManager.Object, EventStoreStreamActor.CreateStreamId(testEvents1[0].AggregateType, testEvents1[0].AggregateId));
            IList<IEvent> result = await testDemoActor.Read(testEvents1.Last().Id, 5);
            CheckValues(testEvents2.Take(5).ToList<IEvent>(), result);
        }

        private static void CheckValues(IList<IEvent> expected, IList<IEvent> actual)
        {
            actual.Should().NotBeNull();
            actual.Count.Should().Be(expected.Count);
            for (int i = 0; i < expected.Count; i++)
            {
                actual[i].Id.Should().Be(expected[i].Id);
                actual[i].CorrelationId.Should().Be(expected[i].CorrelationId);
                actual[i].AggregateId.Should().Be(expected[i].AggregateId);
                actual[i].AggregateType.Should().Be(expected[i].AggregateType);
                actual[i].UserId.Should().Be(expected[i].UserId);
                actual[i].DateTime.Should().Be(expected[i].DateTime);
            }
        }

        private async Task<EventStoreStreamActor> CreateActor(IActorStateManager actorStateManager, string id)
        {
            var actorTypeInformation = ActorTypeInformation.Get(typeof(EventStoreStreamActor));
            EventStoreStreamActor actorFactory(ActorService service, ActorId id) =>
                new EventStoreStreamActor(service, id, actorStateManager);
            var actorService = new ActorService(actorTypeInformation, actorFactory);
            EventStoreStreamActor actor = actorFactory(actorService, new ActorId(id));
            MethodInfo OnActivate = actor.GetType().GetMethod("OnActivateAsync", BindingFlags.NonPublic | BindingFlags.Instance);
            await (Task)OnActivate.Invoke(actor, Array.Empty<object>());
            return actor;
        }
    }
}