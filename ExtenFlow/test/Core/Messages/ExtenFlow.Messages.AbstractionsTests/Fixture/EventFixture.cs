#pragma warning disable CS0612 // Type or member is obsolete

using ExtenFlow.Messages;

namespace ExtenFlow.Domain.AbstractionsTests
{
    public class EventFixture<T> : MessageFixture<T> where T : IEvent
    {
    }
}