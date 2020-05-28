using Dapr.Actors;

using ExtenFlow.Actors;

namespace ExtenFlow.Identity.Roles.Queries
{
    /// <summary>
    /// The role actor interface
    /// </summary>
    /// <seealso cref="IActor"/>
    public interface IRoleRelationalModelActor : IActor, IQueryActor
    {
    }
}