namespace ExtenFlow.Messages
{
    public interface IRequest : IMessage
    {
        string AggregateType { get; }
        string? AggregateId { get; }
    }
}