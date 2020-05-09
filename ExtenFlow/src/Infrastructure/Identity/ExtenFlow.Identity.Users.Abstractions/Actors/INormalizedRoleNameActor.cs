using Dapr.Actors;

using ExtenFlow.Actors;

namespace ExtenFlow.Identity.Users.Actors
{
    /// <summary>
    /// The role actor interface
    /// </summary>
    /// <seealso cref="IActor"/>
    public interface INormalizedUserNameActor : IActor, IDispatchActor
    {
    }
}