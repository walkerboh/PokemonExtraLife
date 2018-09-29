using System.Linq;
using Microsoft.Ajax.Utilities;
using PokemonExtraLifeApi.Models.API;

namespace PokemonExtraLifeApi.EntityFramework
{
    public static class DonationProcessor
    {
        public static (Donation, Pokemon, Trainer) GetNextDonation()
        {
            using (var context = new ExtraLifeContext())
            {
                var nextDonation = context.Donations.FirstOrDefault(d => !d.Processed);

                if (nextDonation == null)
                    return (null, null, null);

                var pokemonOrders = context.PokemonOrders.Include("Pokemon").Include("Trainer").ToList();

                var currentPo = pokemonOrders.First(po => !po.Done);
                var pokemon = currentPo.Pokemon;
                var trainer = currentPo.Trainer;

                pokemon.Damage += nextDonation.Amount;

                Pokemon nextPokemon = null;
                Trainer nextTrainer = null;
                
                if (pokemon.Damage >= pokemon.Health)
                {
                    var nextPo = pokemonOrders.FirstOrDefault(po => po.Sequence == currentPo.Sequence + 1);

                    if (nextPo != null)
                    {
                        nextPokemon = nextPo.Pokemon;
                        nextTrainer = nextPo.Trainer.Id.Equals(trainer.Id) ? null : nextPo.Trainer;
                    }
                }

                nextDonation.Processed = true;

                context.SaveChanges();
                
                return (nextDonation, nextPokemon, nextTrainer);
            }
        }
    }
}