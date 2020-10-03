using System.Threading;
using System.Threading.Tasks;

namespace PokemonExtraLifeApiCore.ExtraLife
{
    public interface IScopedProcessingService
    {
        Task DoWork(CancellationToken stoppingToken);
    }
}