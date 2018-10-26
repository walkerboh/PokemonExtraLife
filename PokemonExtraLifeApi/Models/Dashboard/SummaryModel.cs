using PokemonExtraLifeApi.Models.API;

namespace PokemonExtraLifeApi.Models.Dashboard
{
    public class SummaryModel
    {
        public int TotalDonations { get; set; }

        public decimal TotalDonationAmount { get; set; }
        
        public Pokemon ActivePokemon { get; set; }
        
        public Host ActiveHost { get; set; }

        public decimal DonationGoal { get; set; }
    }
}