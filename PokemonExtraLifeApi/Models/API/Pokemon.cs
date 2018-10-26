using System.Collections.Generic;
using Newtonsoft.Json;

namespace PokemonExtraLifeApi.Models.API
{
    public class Pokemon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public decimal TotalHealth { get; set; }
        public decimal Damage { get; set; }
        public decimal CurrentHealth => TotalHealth - Damage;

        [JsonIgnore]
        public virtual PokemonOrder PokemonOrder { get; set; }
    }
}