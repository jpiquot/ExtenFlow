using System;
using System.Collections.Generic;
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
        /// Determines whether the user has the specified token.
        /// </summary>
        /// <param name="tokenType">The type of the token</param>
        /// <param name="tokenValue">The value of the token</param>
        /// <returns>True if the user has the tokens</returns>
        /// <exception cref="ArgumentNullException">tokenType</exception>
        Task<bool> HasToken(string tokenType, string tokenValue);

        /// <summary>
        /// Adds the user's tokens.
        /// </summary>
        /// <param name="tokenType">The type of the token</param>
        /// <param name="tokenValue">The value of the token</param>
        /// <exception cref="ArgumentNullException">tokenType</exception>
        Task AddToken(string tokenType, string tokenValue);

        /// <summary>
        /// Removes the tokens.
        /// </summary>
        /// <param name="tokenType">The type of the token</param>
        /// <param name="tokenValue">The value of the token</param>
        /// <exception cref="ArgumentNullException">tokenType</exception>
        Task RemoveToken(string tokenType, string tokenValue);

        /// <summary>
        /// Gets the all the user's tokenss.
        /// </summary>
        /// <returns>A list of all tokens as tuples of strings (Token Type, Token Value)</returns>
        Task<IList<Tuple<string, string>>> GetTokens();
    }
}