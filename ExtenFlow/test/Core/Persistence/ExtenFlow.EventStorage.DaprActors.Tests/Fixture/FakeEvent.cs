using System;

using ExtenFlow.Domain;

namespace ExtenFlow.EventStorage.DaprActors.Tests
{
    public class FakeEvent : Event
    {
        public FakeEvent() : base("Test", "001", "Test user", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now)
        {
        }

        public FakeEvent(string aggregateId) : base("Test", aggregateId, "Test user", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now)
        {
        }
    }
}