using System.Collections.Generic;
using System.Data.Entity;
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

                var overkillRemainder = nextDonation.Amount - pokemon.Health + pokemon.Damage;
                pokemon.Damage += nextDonation.Amount;

                Pokemon nextPokemon = null;
                Trainer nextTrainer = null;
                Host nextHost = null;

                var activeGroup = context.ActiveGroup;

                // Force new pokemon if current pokemon is not in newly activated group
                if (pokemon.Damage >= pokemon.Health || (!currentPo.GroupId.HasValue && activeGroup != null) || (currentPo.GroupId.HasValue && activeGroup == null))
                {
                    (nextPokemon, nextTrainer, nextHost) = GetNextItems(context, activeGroup, currentPo, trainer, pokemonOrders);
                }

                nextDonation.Processed = true;

                context.SaveChanges();

                return (nextDonation, nextPokemon, nextTrainer, nextHost);
            }
        }

        private static (Pokemon, Trainer, Host) GetNextItems(ExtraLifeContext context, Group activeGroup, PokemonOrder currentPo, Trainer trainer, IEnumerable<PokemonOrder> pokemonOrders)
        {
            Pokemon nextPokemon = null;
            Trainer nextTrainer = null;
            PokemonOrder nextPo = null;

            currentPo.ForceDone = true;

            if (activeGroup != null)
            {
                var currentPoInGroup = currentPo.GroupId.HasValue && currentPo.GroupId.Value.Equals(activeGroup.Id);
                nextPo = activeGroup.PokemonOrders.FirstOrDefault(po => po.Sequence == (currentPoInGroup ? currentPo.Sequence + 1 : 1));
            }

            // Won't be null if set by group. Will occur if group completes
            if (nextPo == null)
            {
                // Will only occur if group was active but is now complete
                if (currentPo.GroupId.HasValue)
                {
                    nextPo = pokemonOrders.FirstOrDefault(po => po.Sequence == context.PokemonOrders.Include(po1 => po1.Pokemon).ToList().Where(po1 => !po1.GroupId.HasValue && po1.Done).Max(po1 => po1.Sequence) + 1);
                }
                else
                {
                    nextPo = pokemonOrders.FirstOrDefault(po => po.Sequence == currentPo.Sequence + 1 && !po.GroupId.HasValue);
                }
            }

            // If null, we are out of Pokemon
            if (nextPo != null)
            {
                nextPo.Activated = true;
                nextPokemon = nextPo.Pokemon;
                nextTrainer = nextPo.Trainer.Id.Equals(trainer.Id) ? null : nextPo.Trainer;
            }

            var displayStatus = context.GetDisplayStatus();
            displayStatus.CurrentHostId = (displayStatus.CurrentHostId + 1) % (context.Hosts.Count() + 1);

            displayStatus.CurrentHostId = displayStatus.CurrentHostId == 0 ? 1 : displayStatus.CurrentHostId;

            Host nextHost = context.Hosts.First(h => h.Id == displayStatus.CurrentHostId);

            return (nextPokemon, nextTrainer, nextHost);
        }
    }
}