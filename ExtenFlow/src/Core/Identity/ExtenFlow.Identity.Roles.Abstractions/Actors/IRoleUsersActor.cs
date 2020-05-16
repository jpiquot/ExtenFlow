using Dapr.Actors;

using ExtenFlow.Actors;

namespace ExtenFlow.Identity.Roles.Actors
{
    /// <summary>
    /// The role claims actor interface
    /// </summary>
    /// <seealso cref="IActor"/>
    public interface IRoleUsersActor : IActor, IDispatchActor
    {
    }
}