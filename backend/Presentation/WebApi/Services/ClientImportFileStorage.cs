using Application.Common.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace WebApi.Services
{
    public class ClientImportFileStorage : IClientImportFileStorage
    {
        private readonly string _storagePath;

        public ClientImportFileStorage(IWebHostEnvironment environment, IConfiguration configuration)
        {
            var configuredPath = configuration["ClientImport:StoragePath"] ?? "imports/clients";
            _storagePath = Path.Combine(environment.ContentRootPath, configuredPath);
        }

        public async Task<string> SaveAsync(string fileName, Stream content, CancellationToken cancellationToken)
        {
            Directory.CreateDirectory(_storagePath);

            var storedFilePath = Path.Combine(_storagePath, $"{Guid.NewGuid():N}.csv");
            await using var destination = new FileStream(storedFilePath, FileMode.CreateNew, FileAccess.Write, FileShare.None);
            await content.CopyToAsync(destination, cancellationToken);

            return storedFilePath;
        }

        public Stream OpenRead(string filePath)
        {
            return new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        public Task DeleteAsync(string filePath, CancellationToken cancellationToken)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);

            return Task.CompletedTask;
        }
    }
}
