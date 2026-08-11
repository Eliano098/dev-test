using Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WebApi.Services
{
    public class ClientImportBackgroundService : BackgroundService
    {
        private static readonly TimeSpan ProcessingInterval = TimeSpan.FromSeconds(5);
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<ClientImportBackgroundService> _logger;

        public ClientImportBackgroundService(IServiceScopeFactory scopeFactory, ILogger<ClientImportBackgroundService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var processor = scope.ServiceProvider.GetRequiredService<IClientImportProcessor>();
                    await processor.ProcessPendingAsync(stoppingToken);
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "Erro ao processar importações de clientes");
                }

                await Task.Delay(ProcessingInterval, stoppingToken);
            }
        }
    }
}
