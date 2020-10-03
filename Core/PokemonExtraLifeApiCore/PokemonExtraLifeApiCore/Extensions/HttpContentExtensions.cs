using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PokemonExtraLifeApiCore.Extensions
{
    public static class HttpContentExtensions
    {
        public static async Task<T> ReadAsAsync<T>(this HttpContent content)
        {
            return JsonConvert.DeserializeObject<T>(await content.ReadAsStringAsync());
        }
    }
}