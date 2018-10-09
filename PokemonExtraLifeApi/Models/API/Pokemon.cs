using System.Collections.Generic;
using Newtonsoft.Json;

namespace PokemonExtraLifeApi.Models.API
{
    public class Pokemon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public decimal Health { get; set; }
        public decimal Damage { get; set; }

        [JsonIgnore]
        public virtual ICollection<PokemonOrder> PokemonOrders { get; set; }
    }
}