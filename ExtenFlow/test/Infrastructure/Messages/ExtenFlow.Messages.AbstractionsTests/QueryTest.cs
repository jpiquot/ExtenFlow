using System;

using Newtonsoft.Json;

namespace ExtenFlow.Messages.AbstractionsTests
{
    public class TestQuery : Query<string>
    {
        [Obsolete]
        public TestQuery()
        {
        }

        [JsonConstructor]
        public TestQuery(string aggregateType, string aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            : base(aggregateType, aggregateId, userId, correlationId, messageId, dateTime)
        {
        }
    }

    public abstract class QueryBaseTest<TR, TQ> : RequestBaseTest<TQ> where TQ : IQuery<TR>
    {
    }

    public class QueryTest : QueryBaseTest<string, TestQuery>
    {
        protected override TestQuery Create(string aggregateType, string aggregateId, string userId, Guid messageId, Guid correlationId, DateTimeOffset dateTime)
           => new TestQuery(aggregateType, aggregateId, userId, messageId, correlationId, dateTime);

        protected override TestQuery Create()
            => new TestQuery("Agtype", "aggreID", "My user", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now);
    }
}