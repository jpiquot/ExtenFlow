using System;
using System.Collections.Generic;
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

    public class ChangeFakeIntDispatch : Command
    {
        [Obsolete]
        public ChangeFakeIntDispatch()
        {
        }

        public ChangeFakeIntDispatch(Guid fakeGuid, int fakeInt) : base("FakeDispatch", fakeGuid.ToString(), "test-user", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now)
        {
            FakeInt = fakeInt;
        }

        public int FakeInt { get; set; }
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
        public FakeDispatchActor(ActorService actorService, ActorId actorId, IEventBus messageQueue, IActorStateManager actorStateManager = null) : base(actorService, actorId, messageQueue, actorStateManager)
        {
        }

        protected override async Task<IList<IEvent>> ReceiveCommand(ICommand command)
             => command switch
             {
                 CreateFakeDispatch create => await Handle(create),
                 _ => await base.ReceiveCommand(command)
             };

        protected override Task ReceiveEvent(IEvent eventMessage, bool batchSave = false)
            => eventMessage switch
            {
                FakeDispatchCreated created => On(created, batchSave),
                _ => base.ReceiveEvent(eventMessage, batchSave)
            };

        protected override async Task<object> ReceiveQuery(IQuery query)
                            => query switch
                            {
                                GetFakeDispatchInt getFakeInt => await Handle(getFakeInt),
                                _ => await base.ReceiveQuery(query)
                            };

        private Task<IList<IEvent>> Handle(CreateFakeDispatch create)
            => Task.FromResult<IList<IEvent>>(new[] { new FakeDispatchCreated(create.FakeGuid, create.FakeInt, create.FakeString) });

        private Task<int> Handle(GetFakeDispatchInt _)
            => (State == null) ?
                Task.FromException<int>(new NotSupportedException("State not initialized.")) :
                Task.FromResult(State.FakeInt);

        private Task On(FakeDispatchCreated create, bool batchSave)
        {
            if (State == null)
            {
                State = new FakeState();
            }
            State.FakeGuid = create.FakeGuid;
            State.FakeInt = create.FakeInt;
            State.FakeString = create.FakeString;
            if (!batchSave)
            {
                return SetStateData();
            }
            return Task.CompletedTask;
        }
    }
}