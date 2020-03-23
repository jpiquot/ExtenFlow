using System;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

namespace ExtenFlow.Security.DaprStore.Actors
{
    public class UserNormalizedNameIndexActor : Actor, IUserNormalizedNameIndexActor
    {
        private string? _id;

        public UserNormalizedNameIndexActor(ActorService actorService, ActorId actorId, IActorStateManager? actorStateManager = null) : base(actorService, actorId, actorStateManager)
        {
        }

        public Task ReceiveReminderAsync(string reminderName, byte[] state, TimeSpan dueTime, TimeSpan period) => throw new NotImplementedException();

        public Task<string?> GetId() => Task.FromResult(_id);

        public Task Index(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return Task.FromException<string>(new NullReferenceException(nameof(_id)));
            }
            _id = id;
            return Task.CompletedTask;
        }

        public Task Remove(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return Task.FromException<string>(new NullReferenceException(nameof(_id)));
            }
            if (id != _id)
            {
                return Task.FromException<string>(new InvalidOperationException(nameof(_id)));
            }
            _id = null;
            return Task.CompletedTask;
        }
    }
}