using System;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

using ExtenFlow.Messages;
using ExtenFlow.Messages.Dispatcher;

namespace ExtenFlow.Actors.Tests
{
    public interface IFakeDispatchActor : IActor
    {
    }

    public class CreateFakeDispatch : Command
    {
        [Obsolete]
        public CreateFakeDispatch()
        {
        }

        public CreateFakeDispatch(Guid fakeGuid, int fakeInt, string fakeString) : base("FakeDispatch", fakeGuid.ToString(), "test-user", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now)
        {
            FakeGuid = fakeGuid;
            FakeInt = fakeInt;
            FakeString = fakeString;
        }

        public Guid FakeGuid { get; set; }
        public int FakeInt { get; set; }
        public string FakeString { get; set; }
    }

    public class FakeDispatchCreated : Event
    {
        [Obsolete]
        public FakeDispatchCreated()
        {
        }

        public FakeDispatchCreated(Guid fakeGuid, int fakeInt, string fakeString) : base("FakeDispatch", fakeGuid.ToString(), "test-user", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now)
        {
            FakeGuid = fakeGuid;
            FakeInt = fakeInt;
            FakeString = fakeString;
        }

        public Guid FakeGuid { get; set; }
        public int FakeInt { get; set; }
        public string FakeString { get; set; }
    }

    public class FakeDispatchUnknownCommand : Command
    {
        public FakeDispatchUnknownCommand() : base("FakeDispatch", null, "test-user", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now)
        {
        }
    }

    public class FakeDispatchUnknownEvent : Event
    {
        public FakeDispatchUnknownEvent() : base("FakeDispatch", null, "test-user", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now)
        {
        }
    }

    public class FakeDispatchUnknownMessage : Message
    {
        public FakeDispatchUnknownMessage() : base("FakeDispatch", null, "test-user", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now)
        {
        }
    }

    public class FakeDispatchUnknownQuery : Query<int>
    {
        public FakeDispatchUnknownQuery() : base("FakeDispatch", null, "test-user", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now)
        {
        }
    }

    public class GetFakeDispatchInt : Query<int>
    {
        [Obsolete]
        public GetFakeDispatchInt()
        {
        }

        public GetFakeDispatchInt(Guid fakeGuid) : base("FakeDispatch", fakeGuid.ToString(), "test-user", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now)
        {
        }
    }

    internal class FakeDispatchActor : DispatchActorBase<FakeState>, IFakeDispatchActor
    {
        public FakeDispatchActor(ActorService actorService, ActorId actorId, IMessageQueue messageQueue, IActorStateManager actorStateManager = null) : base(actorService, actorId, messageQueue, actorStateManager)
        {
        }

        protected override async Task<object> ReceiveQuery(IQuery query)
            => query switch
            {
                GetFakeDispatchInt getFakeInt => await Handle(getFakeInt),
                _ => await base.ReceiveQuery(query)
            };

        private Task<int> Handle(GetFakeDispatchInt getFakeInt)
            => (State == null) ?
                Task.FromException<int>(new NotSupportedException("State not initialized.")) :
                Task.FromResult(State.FakeInt);
    }
}