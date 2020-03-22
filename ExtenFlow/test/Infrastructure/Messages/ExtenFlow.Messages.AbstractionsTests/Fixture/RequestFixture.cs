#pragma warning disable CS0612 // Type or member is obsolete

namespace ExtenFlow.Messages.AbstractionsTests
{
    public class RequestFixture<T> : MessageFixture<T> where T : IRequest
    {
    }
}