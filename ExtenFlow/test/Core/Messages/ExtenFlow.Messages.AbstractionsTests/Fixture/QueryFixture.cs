#pragma warning disable CS0612 // Type or member is obsolete

using ExtenFlow.Messages;

namespace ExtenFlow.Domain.AbstractionsTests
{
    public class QueryFixture<TR, TQ> : RequestFixture<TQ> where TQ : IQuery<TR>
    {
    }
}