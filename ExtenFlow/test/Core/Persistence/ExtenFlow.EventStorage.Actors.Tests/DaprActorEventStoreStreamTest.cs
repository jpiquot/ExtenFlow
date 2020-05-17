using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ExtenFlow.Domain;

using FluentAssertions;

using Moq;

using Xunit;

namespace ExtenFlow.EventStorage.DaprActors.Tests
{
    public class DaprActorEventStoreStreamTest
    {
        [Fact]
        public async Task DaprActorEventStoreStream_Read_ExpectEventList()
        {
            FakeEvent[] events = new[] { new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent() };
            var actor = new Mock<IEventStoreStreamActor>();
            var stream = new DaprActorEventStoreStream(actor.Object);
            actor.Setup(system => system.Read(null, 0))
                .Returns(Task.FromResult<IList<IEvent>>(events))
                .Verifiable();
            (await stream.Read())
                .Should()
                .Equal((IList<IEvent>)events);

            actor.VerifyAll();
        }

        [Fact]
        public async Task DaprActorEventStoreStream_ReadFiveAfter_ExpectFiveAfterList()
        {
            var afterId = Guid.NewGuid();
            FakeEvent[] events = new[] { new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent(), new FakeEvent() };
            var actor = new Mock<IEventStoreStreamActor>();
            var stream = new DaprActorEventStoreStream(actor.Object);
            actor.Setup(system => system.Read(afterId, 5))
                .Returns(Task.FromResult<IList<IEvent>>(events))
                .Verifiable();
            (await stream.Read(afterId, 5))
                .Should()
                .Equal((IList<IEvent>)events);

            actor.VerifyAll();
        }
    }
}