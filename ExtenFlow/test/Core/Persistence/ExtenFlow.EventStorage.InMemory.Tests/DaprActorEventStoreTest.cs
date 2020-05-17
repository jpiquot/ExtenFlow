using System.Threading.Tasks;

using ExtenFlow.Actors;

using Moq;

using Xunit;

namespace ExtenFlow.EventStorage.DaprActors.Tests
{
    public class DaprActorEventStoreTest
    {
        [Fact]
        public async Task DaprActorEventStore_GetStream_ExpectActorSystemCreate()
        {
            const string aggregateType = "Test Aggregate";
            const string aggregateId = "0000202";
            var actorSystem = new Mock<IActorSystem>();
            var store = new DaprActorEventStore(() => actorSystem.Object);
            actorSystem.Setup(system => system.Create<IEventStoreStreamActor, string>($"{aggregateType}-[{aggregateId}]"))
                .Verifiable();
            await store.GetStoreStream(aggregateType, aggregateId);

            actorSystem.VerifyAll();
        }
    }
}