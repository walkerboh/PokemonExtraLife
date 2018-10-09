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
                
                var pokemonOrders = context.PokemonOrders.Include("Pokemon").Include("Trainer").ToList();

                var currentPo = pokemonOrders.First(po => po.Activated && !po.Done);
                var pokemon = currentPo.Pokemon;
                var trainer = currentPo.Trainer;

                pokemon.Damage += nextDonation.Amount;

                Pokemon nextPokemon = null;
                Trainer nextTrainer = null;
                Host nextHost = null;
                
                var activeGroup = context.ActiveGroup;
                
                // Force new pokemon if current pokemon is not in newly activated group
                if (pokemon.Damage >= pokemon.Health || (!currentPo.GroupId.HasValue && activeGroup != null) || (currentPo.GroupId.HasValue && activeGroup == null))
                {
                    PokemonOrder nextPo = null;

                    if (activeGroup != null)
                    {
                        var currentPoInGroup = currentPo.GroupId.HasValue && currentPo.GroupId.Value.Equals(activeGroup.Id);
                        nextPo = activeGroup.PokemonOrders.FirstOrDefault(po => po.Sequence == (currentPoInGroup ? currentPo.Sequence + 1 : 1));
                    }
                    
                    // Won't be null if set by group. Will occur if group completes
                    if(nextPo == null)
                    {
                        // Will only occur if group was active but is now complete
                        if (activeGroup != null)
                        {
                            nextPo = pokemonOrders.FirstOrDefault(po => po.Sequence == context.PokemonOrders.Where(po1 => !po1.GroupId.HasValue && po1.Done).Max(po1 => po1.Sequence) + 1);
                        }
                        else
                        {
                            nextPo = pokemonOrders.FirstOrDefault(po => po.Sequence == currentPo.Sequence + 1);                            
                        }
                    }

                    // If null, we are out of Pokemon
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