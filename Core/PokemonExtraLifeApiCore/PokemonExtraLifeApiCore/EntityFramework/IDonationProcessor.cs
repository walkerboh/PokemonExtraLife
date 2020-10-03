using PokemonExtraLifeApiCore.Models.API;

namespace PokemonExtraLifeApiCore.EntityFramework
{
    public interface IDonationProcessor
    {
        public IDonationDisplayModel GetNextDonation();
    }
}