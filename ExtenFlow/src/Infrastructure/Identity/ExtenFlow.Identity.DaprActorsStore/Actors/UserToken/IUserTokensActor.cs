using System.Threading.Tasks;

using Dapr.Actors;

namespace ExtenFlow.Identity.DaprActorsStore
{
    /// <summary>
    /// The user tokens actor interface
    /// </summary>
    /// <seealso cref="IActor"/>
    public interface IUserTokensActor : IActor
    {
        /// <summary>
        /// Adds the specified token.
        /// </summary>
        /// <param name="loginProvider">The login provider.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        Task Add(string loginProvider, string name, string value);

        /// <summary>
        /// Removes the specified token.
        /// </summary>
        /// <param name="loginProvider">The login provider.</param>
        /// <param name="name">The name.</param>
        Task Remove(string loginProvider, string name);

        /// <summary>
        /// Finds the token value.
        /// </summary>
        /// <param name="loginProvider">The login provider.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        Task<string?> FindValue(string loginProvider, string name);
    }
}