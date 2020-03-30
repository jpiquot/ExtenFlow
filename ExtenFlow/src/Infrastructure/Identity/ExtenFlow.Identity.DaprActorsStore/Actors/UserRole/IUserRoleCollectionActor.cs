using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Dapr.Actors;

namespace ExtenFlow.Identity.DaprActorsStore
{
    /// <summary>
    /// User roles collection actor
    /// </summary>
    /// <seealso cref="IActor"/>
    public interface IUserRoleCollectionActor : IActor
    {
        Task Create(Guid userId, Guid roleId);

        Task Delete(Guid userId, Guid roleId);

        Task<bool> Exist(Guid userId, Guid roleId);

        Task<IList<Guid>> GetRoleIds(Guid userId);

        Task<IList<Guid>> GetUserIds(Guid roleId);
    }
}