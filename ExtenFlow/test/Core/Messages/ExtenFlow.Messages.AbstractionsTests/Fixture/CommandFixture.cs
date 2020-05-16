#pragma warning disable CS0612 // Type or member is obsolete

namespace ExtenFlow.Domain.AbstractionsTests
{
    public class CommandFixture<T> : RequestFixture<T> where T : ICommand
    {
    }
}