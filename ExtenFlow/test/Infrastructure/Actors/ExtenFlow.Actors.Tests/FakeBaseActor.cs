using System;

using Dapr.Actors;
using Dapr.Actors.Runtime;

namespace ExtenFlow.Actors.Tests
{
    public interface IFakeBaseActor : IActor
    {
    }

    internal class FakeBaseActor : ActorBase<FakeState>, IFakeBaseActor
    {
        public FakeBaseActor(ActorService actorService, ActorId actorId, IActorStateManager actorStateManager = null) : base(actorService, actorId, actorStateManager)
        {
        }
    }

    internal class FakeState
    {
        public Guid FakeGuid { get; set; }
        public int FakeInt { get; set; }
        public string FakeString { get; set; }
    }
}