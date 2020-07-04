using ExtenFlow.IdentityServer.Domain.Models;

using Microsoft.EntityFrameworkCore;

namespace ExtenFlow.IdentityServer.Queries
{
    /// <summary>
    /// Class RoleDetailsContext. Implements the <see cref="Microsoft.EntityFrameworkCore.DbContext"/>
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.DbContext"/>
    public class RoleDetailsContext : DbContext
    {
        /// <summary>
        /// Gets the roles.
        /// </summary>
        /// <value>The roles.</value>
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        public DbSet<Role> Roles { get; set; }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
    }
}