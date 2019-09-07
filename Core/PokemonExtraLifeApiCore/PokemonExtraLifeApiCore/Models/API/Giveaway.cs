using Newtonsoft.Json;
using PokemonExtraLifeApiCore.Enum;

namespace PokemonExtraLifeApiCore.Models.API
{
    public class Giveaway
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string PrizeName { get; set; }
        public string Contributor { get; set; }
        public string Url { get; set; }

        [JsonIgnore]
        public Gym Gym { get; set; }
    }
}