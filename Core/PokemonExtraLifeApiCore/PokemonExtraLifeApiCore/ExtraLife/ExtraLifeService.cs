using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PokemonExtraLifeApiCore.ExtraLife
{
    public class ExtraLifeService : BackgroundService
    {
        private readonly IServiceProvider _provider;
        private readonly ILogger _logger;

        public ExtraLifeService(IServiceProvider provider, ILogger logger)
        {
            _provider = provider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("ExtraLife service is starting.");

            await DoWork(stoppingToken);
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            _logger.LogInformation("ExtraLife service is working");

            using var scope = _provider.CreateScope();
            var scopedProcessingService = scope.ServiceProvider.GetRequiredService<IScopedProcessingService>();

            await scopedProcessingService.DoWork(stoppingToken);
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("ExtraLife service is stopping");

            await base.StopAsync(stoppingToken);
        }
    }
}