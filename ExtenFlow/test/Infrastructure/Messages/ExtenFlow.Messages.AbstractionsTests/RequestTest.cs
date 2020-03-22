using System;

using Newtonsoft.Json;

namespace ExtenFlow.Messages.AbstractionsTests
{
    public class TestRequest : Request
    {
        [Obsolete]
        public TestRequest()
        {
        }

        [JsonConstructor]
        public TestRequest(string aggregateType, string aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            : base(aggregateType, aggregateId, userId, correlationId, messageId, dateTime)
        {
        }
    }

    public abstract class RequestBaseTest<T> : MessageBaseTest<T> where T : IRequest
    {
    }

    public class RequestTest : RequestBaseTest<TestRequest>
    {
        protected override TestRequest Create(string aggregateType, string aggregateId, string userId, Guid messageId, Guid correlationId, DateTimeOffset dateTime)
           => new TestRequest(aggregateType, aggregateId, userId, messageId, correlationId, dateTime);

        protected override TestRequest Create()
            => new TestRequest("Agtype", "aggreID", "My user", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now);
    }
}