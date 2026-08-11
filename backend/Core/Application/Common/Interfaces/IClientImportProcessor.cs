using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IClientImportProcessor
    {
        Task ProcessPendingAsync(CancellationToken cancellationToken);
    }
}
