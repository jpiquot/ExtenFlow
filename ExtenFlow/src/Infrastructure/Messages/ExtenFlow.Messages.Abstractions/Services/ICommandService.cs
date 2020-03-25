using System.Threading;
using System.Threading.Tasks;

namespace ExtenFlow.Messages.Services
{
    public interface ICommandService
    {
        Task Invoke(ICommand command, CancellationToken cancellationToken = default);
    }
}