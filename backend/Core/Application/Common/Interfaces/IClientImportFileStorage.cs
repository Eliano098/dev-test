using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IClientImportFileStorage
    {
        Task<string> SaveAsync(string fileName, Stream content, CancellationToken cancellationToken);
        Stream OpenRead(string filePath);
        Task DeleteAsync(string filePath, CancellationToken cancellationToken);
    }
}
