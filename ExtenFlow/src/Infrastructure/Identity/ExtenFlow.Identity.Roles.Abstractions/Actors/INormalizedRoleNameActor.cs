using Dapr.Actors;

using ExtenFlow.Actors;

namespace ExtenFlow.Identity.Roles.Actors
{
    /// <summary>
    /// The role actor interface
    /// </summary>
    /// <seealso cref="IActor"/>
    public interface INormalizedRoleNameActor : IActor, IDispatchActor
    {
    }
}