using PokemonExtraLifeApiCore.Models.API;

namespace PokemonExtraLifeApiCore.Models.Dashboard
{
    public class SummaryModel
    {
        public int TotalDonations { get; set; }

        public decimal TotalDonationAmount { get; set; }

        public Pokemon ActivePokemon { get; set; }

        public Host ActiveHost { get; set; }

        public decimal DonationGoal { get; set; }

        public bool TrackDonations { get; set; }

        public decimal HealthMultiplier { get; set; }
    }
}