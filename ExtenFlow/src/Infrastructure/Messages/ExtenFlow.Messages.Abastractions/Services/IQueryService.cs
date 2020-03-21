using System.Threading;
using System.Threading.Tasks;

namespace ExtenFlow.Messages.Services
{
    public interface IQueryService
    {
        Task<T> Invoke<T>(IQuery<T> query, CancellationToken cancellationToken = default);
    }
}