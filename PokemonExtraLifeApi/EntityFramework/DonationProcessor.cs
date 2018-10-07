using System.Linq;
using Microsoft.Ajax.Utilities;
using PokemonExtraLifeApi.Models.API;

namespace PokemonExtraLifeApi.EntityFramework
{
    public static class DonationProcessor
    {
        public static (Donation, Pokemon, Trainer, Host) GetNextDonation()
        {
            using (var context = new ExtraLifeContext())
            {
                var nextDonation = context.Donations.FirstOrDefault(d => !d.Processed);

                if (nextDonation == null)
                    return (null, null, null, null);

                var activeGroup = context.ActiveGroup;
                
                
                
                var pokemonOrders = context.PokemonOrders.Include("Pokemon").Include("Trainer").ToList();

                var currentPo = pokemonOrders.First(po => !po.Done);
                var pokemon = currentPo.Pokemon;
                var trainer = currentPo.Trainer;

                pokemon.Damage += nextDonation.Amount;

                Pokemon nextPokemon = null;
                Trainer nextTrainer = null;
                Host nextHost = null;
                
                if (pokemon.Damage >= pokemon.Health)
                {
                    var nextPo = pokemonOrders.FirstOrDefault(po => po.Sequence == currentPo.Sequence + 1);

                    if (nextPo != null)
                    {
                        nextPokemon = nextPo.Pokemon;
                        nextTrainer = nextPo.Trainer.Id.Equals(trainer.Id) ? null : nextPo.Trainer;
                    }

                    var displayStatus = context.GetDisplayStatus();
                    displayStatus.CurrentHostId = (displayStatus.CurrentHostId + 1) % context.Hosts.Count();
                    
                    nextHost = context.Hosts.First(h => h.Id == displayStatus.CurrentHostId);
                }

                nextDonation.Processed = true;

                context.SaveChanges();
                
                return (nextDonation, nextPokemon, nextTrainer, nextHost);
            }
        }
    }
}