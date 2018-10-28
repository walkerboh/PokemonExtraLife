using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using PokemonExtraLifeApi.Models.API;

namespace PokemonExtraLifeApi.EntityFramework
{
    public static class DonationProcessor
    {
        public static (Donation, Pokemon, Pokemon, Trainer, Host) GetNextDonation()
        {
            using (ExtraLifeContext context = new ExtraLifeContext())
            {
                Donation nextDonation = context.Donations.FirstOrDefault(d => !d.Processed);

                if (nextDonation == null)
                    return (null, null, null, null, null);

                List<PokemonOrder> pokemonOrders = context.PokemonOrders.Include("Pokemon").Include("Trainer.PokemonOrders.Pokemon").ToList();

                PokemonOrder currentPo = pokemonOrders.FirstOrDefault(po => po.Activated && !po.Done);

                if (currentPo == null) return (nextDonation, null, null, null, null);

                Pokemon pokemon = currentPo.Pokemon;
                Trainer trainer = currentPo.Trainer;

                decimal overkillRemainder = nextDonation.Amount - pokemon.TotalHealth + pokemon.Damage;
                pokemon.Damage += nextDonation.Amount;

                Pokemon nextPokemon = null;
                Trainer nextTrainer = null;
                Host nextHost = null;

                Group activeGroup = context.ActiveGroup;

                // New pokemon if KOed or group is ended and current PO is in a group
                if (pokemon.Damage >= pokemon.TotalHealth || currentPo.GroupId.HasValue && activeGroup == null)
                {
                    (nextPokemon, nextTrainer, nextHost) = GetNextItems(context, activeGroup, currentPo, trainer, pokemonOrders);
                }

                if (nextPokemon?.PokemonOrder?.Trainer != null && !nextPokemon.PokemonOrder.Trainer.Leader && overkillRemainder >= 10)
                {
                    decimal damage = Math.Floor(overkillRemainder / 2.5m);
                    nextPokemon.Damage += Math.Min(damage, nextPokemon.TotalHealth - 1);
                }

                nextDonation.Processed = true;

                context.SaveChanges();

                return (nextDonation, pokemon, nextPokemon, nextTrainer, nextHost);
            }
        }

        private static (Pokemon, Trainer, Host) GetNextItems(ExtraLifeContext context, Group activeGroup, PokemonOrder currentPo, Trainer trainer, IEnumerable<PokemonOrder> pokemonOrders)
        {
            Pokemon nextPokemon = null;
            Trainer nextTrainer = null;
            PokemonOrder nextPo = null;

            currentPo.ForceDone = true;

            if (activeGroup != null && (currentPo.GroupId == activeGroup.Id || trainer.Done))
            {
                if (!activeGroup.StartTime.HasValue)
                {
                    activeGroup.StartTime = DateTime.Now;
                }

                nextPo = pokemonOrders.Where(po => po.GroupId == activeGroup.Id).OrderBy(po => po.Sequence).FirstOrDefault(po => !po.Activated);
            }
            else
            {
                nextPo = pokemonOrders.Where(po => !po.GroupId.HasValue).OrderBy(po => po.Sequence).FirstOrDefault(po => !po.Activated);
            }

            // If null, we are out of Pokemon
            if (nextPo != null)
            {
                nextPo.Activated = true;
                nextPokemon = nextPo.Pokemon;
                nextTrainer = nextPo.Trainer.Id.Equals(trainer.Id) ? null : nextPo.Trainer;
            }

            DisplayStatus displayStatus = context.GetDisplayStatus();
            displayStatus.CurrentHostId = (displayStatus.CurrentHostId + 1) % (context.Hosts.Count() + 1);

            displayStatus.CurrentHostId = displayStatus.CurrentHostId == 0 ? 1 : displayStatus.CurrentHostId;

            Host nextHost = context.Hosts.First(h => h.Id == displayStatus.CurrentHostId);

            return (nextPokemon, nextTrainer, nextHost);
        }
    }
}