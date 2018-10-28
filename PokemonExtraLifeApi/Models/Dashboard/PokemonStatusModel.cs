using PokemonExtraLifeApi.Models.API;

namespace PokemonExtraLifeApi.Models.Dashboard
{
    public class PokemonStatusModel
    {
        public Pokemon CurrentPokemon { get; set; }

        public Trainer CurrentTrainer { get; set; }

        public Gym? CurrentGym { get; set; }
    }
}