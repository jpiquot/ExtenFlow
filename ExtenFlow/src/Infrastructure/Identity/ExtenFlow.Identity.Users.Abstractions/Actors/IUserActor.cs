using Dapr.Actors;

using ExtenFlow.Actors;

namespace ExtenFlow.Identity.Users.Actors
{
    /// <summary>
    /// The user actor interface
    /// </summary>
    /// <seealso cref="IActor"/>
    public interface IUserActor : IActor, IDispatchActor
    {
    }
}