using System;
using System.Collections.Generic;
using System.Linq;

using ExtenFlow.Messages;

using FluentAssertions;

using Xunit;

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