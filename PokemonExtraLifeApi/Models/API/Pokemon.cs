using System.Collections.Generic;
using Newtonsoft.Json;

namespace PokemonExtraLifeApi.Models.API
{
    public class Pokemon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public double Health { get; set; }
        public double Damage { get; set; } = 0;

        [JsonIgnore]
        public virtual ICollection<PokemonOrder> PokemonOrders { get; set; }
    }
}