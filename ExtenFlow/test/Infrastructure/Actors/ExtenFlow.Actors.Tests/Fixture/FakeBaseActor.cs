using System;
using System.Collections.Generic;
using Dapr.Actors;
using Dapr.Actors.Runtime;
using ExtenFlow.Infrastructure;

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

    internal class FakeState : ValueObject
    {
        public Guid FakeGuid { get; set; }
        public int FakeInt { get; set; }
        public string FakeString { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return FakeGuid;
            yield return FakeInt;
            yield return FakeString;
        }
    }
}