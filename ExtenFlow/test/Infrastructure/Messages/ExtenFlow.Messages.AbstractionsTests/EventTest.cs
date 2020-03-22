using System;

using Newtonsoft.Json;

namespace ExtenFlow.Messages.AbstractionsTests
{
    public class TestEvent : Event
    {
        [Obsolete]
        public TestEvent()
        {
        }

        [JsonConstructor]
        public TestEvent(string aggregateType, string aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime) : base(aggregateType, aggregateId, userId, correlationId, messageId, dateTime)
        {
        }
    }

    public abstract class EventBaseTest<T> : MessageBaseTest<T> where T : IEvent
    {
    }

    public class EventTest : EventBaseTest<TestEvent>
    {
        protected override TestEvent Create(string aggregateType, string aggregateId, string userId, Guid messageId, Guid correlationId, DateTimeOffset dateTime)
           => new TestEvent(aggregateType, aggregateId, userId, messageId, correlationId, dateTime);

        protected override TestEvent Create()
            => new TestEvent("Agtype", "aggreID", "My user", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now);
    }
}