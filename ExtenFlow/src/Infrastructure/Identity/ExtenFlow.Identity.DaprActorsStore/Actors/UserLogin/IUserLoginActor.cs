using System.Threading.Tasks;

using Dapr.Actors;

using ExtenFlow.Identity.Models;

namespace ExtenFlow.Identity.DaprActorsStore
{
    /// <summary>
    /// The user login actor interface
    /// </summary>
    /// <seealso cref="IActor"/>
    public interface IUserLoginActor : IActor
    {
        /// <summary>
        /// Gets the user login.
        /// </summary>
        /// <returns>The user login object</returns>
        Task<UserLogin> GetUserLogin();
    }
}