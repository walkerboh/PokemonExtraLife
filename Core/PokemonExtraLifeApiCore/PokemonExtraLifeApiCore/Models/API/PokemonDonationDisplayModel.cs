namespace PokemonExtraLifeApiCore.Models.API
{
    public class PokemonDonationDisplayModel : IDonationDisplayModel
    {
        public Donation Donation { get; set; }

        public Pokemon CurrentPokemon { get; set; }
        
        public Pokemon NextPokemon { get; set; }

        public Trainer CurrentTrainer { get; set; }

        public Trainer NextTrainer { get; set; }

        public Host CurrentHost { get; set; }

        public Host NextHost { get; set; }

        public bool Done { get; set; }
    }
}