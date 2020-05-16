using ExtenFlow.Identity.Users.Models;

using Microsoft.AspNetCore.Identity;

namespace ExtenFlow.Identity.Users.Stores
{
    /// <summary>
    /// User store interface
    /// </summary>
    /// <seealso cref="User"/>
    public interface IUserStore : IUserStore<User>, IQueryableUserStore<User>, IUserClaimStore<User>
    {
    }
}