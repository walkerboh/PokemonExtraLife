using System;
using Newtonsoft.Json;

namespace PokemonExtraLifeApi.Models.API
{
    public class Donation
    {
        public int Id { get; set; }
        public string Donor { get; set; }
        public decimal Amount { get; set; }
        public DateTime Time { get; set; }
        public string Message { get; set; }
        public Gym? Gym { get; set; }
        [JsonIgnore]
        public bool Processed { get; set; }
    }
}