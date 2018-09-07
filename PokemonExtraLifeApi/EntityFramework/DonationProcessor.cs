using System.Linq;
using Microsoft.Ajax.Utilities;
using PokemonExtraLifeApi.Models.API;

namespace PokemonExtraLifeApi.EntityFramework
{
    public static class DonationProcessor
    {
        public static (Donation, Pokemon) GetNextDonation()
        {
            using (var context = new ExtraLifeContext())
            {
                var nextDonation = context.Donations.FirstOrDefault(d => !d.Processed);

                if (nextDonation == null)
                    return (null, null);

                var pokemonOrders = context.PokemonOrders.Include("Pokemon").Include("Trainer").ToList();

                var nextPo = pokemonOrders.First(po => !po.Done);
                var pokemon = nextPo.Pokemon;
                var trainer = nextPo.Trainer;

                pokemon.Damage += nextDonation.Amount;

                Pokemon nextPokemon = null;
                
                if (pokemon.Damage >= pokemon.Health)
                {
                    nextPokemon = pokemonOrders.First(po => po.Sequence == nextPo.Sequence + 1).Pokemon;
                }

                nextDonation.Processed = true;

                context.SaveChanges();
                
                return (nextDonation, nextPokemon);
            }
        }
    }
}