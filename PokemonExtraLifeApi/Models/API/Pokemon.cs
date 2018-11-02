using System;
using Newtonsoft.Json;

namespace PokemonExtraLifeApi.Models.API
{
    public class Pokemon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public decimal StartingHealth { get; set; }
        public decimal Damage { get; set; }
        public decimal HealthMultiplier { get; set; } = 1;
        public decimal TotalHealth => Math.Floor(StartingHealth * HealthMultiplier);
        public decimal CurrentHealth => TotalHealth - Damage;

        [JsonIgnore]
        public virtual PokemonOrder PokemonOrder { get; set; }
    }
}