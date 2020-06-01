using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

using ExtenFlow.Messages;
using ExtenFlow.Messages.Events;

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

        public ChangeFakeIntDispatch(Guid fakeGuid, int fakeInt) : base("FakeDispatch", fakeGuid.ToString(), "test-user")
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

        public CreateFakeDispatch(Guid fakeGuid, int fakeInt, string fakeString) : base("FakeDispatch", fakeGuid.ToString(), "test-user")
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

        public FakeDispatchCreated(Guid fakeGuid, int fakeInt, string fakeString) : base("FakeDispatch", fakeGuid.ToString(), "test-user")
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
        public FakeDispatchUnknownCommand() : base("FakeDispatch", string.Empty, "test-user")
        {
        }
    }

    public class FakeDispatchUnknownEvent : Event
    {
        public FakeDispatchUnknownEvent() : base("FakeDispatch", string.Empty, "test-user")
        {
        }
    }

    public class FakeDispatchUnknownMessage : Message
    {
        public FakeDispatchUnknownMessage() : base("FakeDispatch", string.Empty, "test-user")
        {
        }
    }

    public class FakeDispatchUnknownQuery : Query<int>
    {
        public FakeDispatchUnknownQuery() : base("FakeDispatch", string.Empty, "test-user")
        {
        }
    }

    public class FakeNotify : Message
    {
        [Obsolete]
        public FakeNotify()
        {
        }

        public FakeNotify(Guid fakeGuid, int fakeInt) : base("FakeDispatch", fakeGuid.ToString(), "test-user")
        {
            FakeInt = fakeInt;
        }

        public int FakeInt { get; set; }
    }

    public class GetFakeDispatchInt : Query<int>
    {
        public GetFakeDispatchInt(Guid fakeGuid) : base("FakeDispatch", fakeGuid.ToString(), "test-user")
        {
        }
    }

    internal class FakeDispatchActor : DispatchActorBase, IFakeDispatchActor
    {
        private Guid? _fakeGuid;
        private int? _fakeInt;
        private string _fakeString;

        public FakeDispatchActor(ActorService actorService, ActorId actorId, IEventPublisher eventPublisher, IActorStateManager actorStateManager = null) : base(actorService, actorId, eventPublisher, actorStateManager)
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
                FakeDispatchCreated created => On(created),
                _ => base.ReceiveEvent(eventMessage, batchSave)
            };

        protected override Task ReceiveNotification(IMessage message)
            => message switch
            {
                FakeDispatchCreated created => On(created),
                _ => base.ReceiveNotification(message)
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
            => (_fakeInt == null) ?
                Task.FromException<int>(new NotSupportedException("State not initialized.")) :
                Task.FromResult<int>(_fakeInt.Value);

        private Task On(FakeDispatchCreated create)
        {
            _fakeGuid = create.FakeGuid;
            _fakeInt = create.FakeInt;
            _fakeString = create.FakeString;
            return Task.CompletedTask;
        }
    }
}