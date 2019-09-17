using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using PokemonExtraLifeApiCore.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PokemonExtraLifeApiCore.ExtraLife
{
    public class ExtraLifeService : BackgroundService
    {
        private readonly ExtraLifeApiSettings _settings;
        private readonly IServiceProvider _provider;

        public ExtraLifeService(IOptions<ExtraLifeApiSettings> settings, IServiceProvider provider)
        {
            _settings = settings.Value;
            _provider = provider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                using(var scope = _provider.CreateScope())
                {
                    var scopedService = scope.ServiceProvider.GetRequiredService<IScopedProcessingService>();

                    await scopedService.DoWork();
                }

                await Task.Delay(_settings.RequestDelay, stoppingToken);
            }
        }

        
    }
}