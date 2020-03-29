using System.Threading.Tasks;

using Dapr.Actors;

using ExtenFlow.Identity.Models;

namespace ExtenFlow.Identity.DaprActorsStore
{
    /// <summary>
    /// The user role actor interface
    /// </summary>
    /// <seealso cref="IActor"/>
    public interface IUserRoleActor : IActor
    {
        /// <summary>
        /// Gets the user role.
        /// </summary>
        /// <returns>The user role object</returns>
        Task<UserRole> GetUserRole();
        void Create();
        void Delete();
    }
}