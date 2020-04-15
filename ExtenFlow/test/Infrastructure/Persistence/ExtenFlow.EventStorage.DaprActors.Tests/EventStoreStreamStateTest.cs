using System;
using System.Collections.Generic;
using System.Linq;

using ExtenFlow.Messages;

using FluentAssertions;

using Xunit;

using static FluentAssertions.FluentActions;

namespace ExtenFlow.EventStorage.DaprActors.Tests
{
    public class EventStoreStreamStateTest
    {
        [Fact]
        public void EmptyStateAppend_ExpectSameList()
        {
            FakeEvent[] testEvents = new[] { new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent() };
            var store = new EventStoreStreamState();
            store.Append(testEvents);
            IList<IEvent> result = store.Read();
            CheckValues(testEvents, result);
        }

        [Fact]
        public void NewState_ExpectEmptyList()
        {
            IList<IEvent> result = new EventStoreStreamState().Read();

            result.Should().NotBeNull();
            result.Count.Should().Be(0);
        }

        [Fact]
        public void StateAppend_ExpectSumList()
        {
            FakeEvent[] testEvents = new[] { new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent() };
            var store = new EventStoreStreamState();
            store.Append(testEvents.Take(4).ToList<IEvent>());
            store.Append(testEvents.Skip(4).Take(2).ToList<IEvent>());
            store.Append(testEvents.Skip(6).ToList<IEvent>());
            IList<IEvent> result = store.Read();
            CheckValues(testEvents, result);
        }

        [Fact]
        public void StateReadAfter_ExpectAfterEvents()
        {
            FakeEvent[] testEvents1 = new[] { new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent() };
            FakeEvent[] testEvents2 = new[] { new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent() };
            var store = new EventStoreStreamState();
            store.Append(testEvents1);
            store.Append(testEvents2);
            IList<IEvent> result = store.Read(testEvents1.Last().MessageId);
            CheckValues(testEvents2, result);
        }

        [Fact]
        public void StateReadAfterUnknown_ExpectException()
        {
            FakeEvent[] testEvents = new[] { new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent() };
            var store = new EventStoreStreamState();
            store.Append(testEvents);
            Invoking(() => store.Read(Guid.NewGuid())).Should().Throw<ArgumentException>();
        }

        [Fact]
        public void StateReadFiveAfter_ExpectFiveAfterEvents()
        {
            FakeEvent[] testEvents1 = new[] { new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent() };
            FakeEvent[] testEvents2 = new[] { new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent() };
            var store = new EventStoreStreamState();
            store.Append(testEvents1);
            store.Append(testEvents2);
            IList<IEvent> result = store.Read(testEvents1.Last().MessageId, 5);
            CheckValues(testEvents2.Take(5).ToList<IEvent>(), result);
        }

        [Fact]
        public void StateReadFiveEvents_ExpectFiveEvents()
        {
            FakeEvent[] testEvents = new[] { new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent() };
            var store = new EventStoreStreamState();
            store.Append(testEvents);
            IList<IEvent> result = store.Read(null, 5);
            CheckValues(testEvents.Take(5).ToList<IEvent>(), result);
        }

        private static void CheckValues(IList<IEvent> expected, IList<IEvent> actual)
        {
            actual.Should().NotBeNull();
            actual.Count.Should().Be(expected.Count);
            for (int i = 0; i < expected.Count; i++)
            {
                actual[i].MessageId.Should().Be(expected[i].MessageId);
                actual[i].CorrelationId.Should().Be(expected[i].CorrelationId);
                actual[i].AggregateId.Should().Be(expected[i].AggregateId);
                actual[i].AggregateType.Should().Be(expected[i].AggregateType);
                actual[i].UserId.Should().Be(expected[i].UserId);
                actual[i].DateTime.Should().Be(expected[i].DateTime);
            }
        }
    }
}