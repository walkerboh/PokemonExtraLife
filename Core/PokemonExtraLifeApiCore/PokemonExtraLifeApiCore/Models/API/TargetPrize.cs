using Newtonsoft.Json;

namespace PokemonExtraLifeApiCore.Models.API
{
    public class TargetPrize
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Contributor { get; set; }
        public string Url { get; set; }
        public decimal Target { get; set; }
        public bool Activated { get; set; } = true;
        public int? DonationId { get; set; }

        public virtual Donation Donation { get; set; }
    }
}