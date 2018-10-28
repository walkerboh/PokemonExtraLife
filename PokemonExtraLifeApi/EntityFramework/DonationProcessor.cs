using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices;
using PokemonExtraLifeApi.Models.API;

namespace PokemonExtraLifeApi.EntityFramework
{
    public static class DonationProcessor
    {
        public static (Donation, Pokemon, Pokemon, Trainer, Trainer, Host, Host) GetNextDonation()
        {
            using (ExtraLifeContext context = new ExtraLifeContext())
            {
                Donation nextDonation = context.Donations.OrderBy(d=>d.Time).FirstOrDefault(d => !d.Processed);

                List<PokemonOrder> pokemonOrders = context.PokemonOrders.Include("Pokemon").Include("Trainer.PokemonOrders.Pokemon").ToList();

                PokemonOrder currentPo = pokemonOrders.FirstOrDefault(po => po.Activated && !po.Done);

                // If we don't have an active PO (first or some weird case) try to find one not in a group and pick it
                if(currentPo == null && pokemonOrders.Any(po=>!po.GroupId.HasValue && !po.Activated))
                {
                    currentPo = pokemonOrders.FirstOrDefault(po => !po.GroupId.HasValue && !po.Activated);
                }
                
                // if we still don't have a PO, just return the donation
                if (currentPo == null)
                {
                    return (nextDonation, null, null, null, null, null, null);
                }

                DisplayStatus displayStatus = context.GetDisplayStatus();
                Pokemon pokemon = currentPo.Pokemon;
                Trainer trainer = currentPo.Trainer;
                Pokemon nextPokemon = null;
                Trainer nextTrainer = null;
                Host nextHost = null;
                Host currentHost = context.Hosts.First(h => h.Id == displayStatus.CurrentHostId);
                
                if (nextDonation != null)
                {
                    decimal overkillRemainder = nextDonation.Amount - pokemon.TotalHealth + pokemon.Damage;
                    pokemon.Damage += nextDonation.Amount;

                    Group activeGroup = context.ActiveGroup;

                    // New pokemon if KOed or group is ended and current PO is in a group
                    if (pokemon.Damage >= pokemon.TotalHealth || currentPo.GroupId.HasValue && activeGroup == null)
                    {
                        (nextPokemon, nextTrainer, nextHost) = GetNextItems(context, activeGroup, currentPo, trainer, pokemonOrders, displayStatus);
                    }

                    if (nextPokemon?.PokemonOrder?.Trainer != null && !nextPokemon.PokemonOrder.Trainer.Leader && overkillRemainder >= 10)
                    {
                        decimal damage = Math.Floor(overkillRemainder / 2.5m);
                        nextPokemon.Damage += Math.Min(damage, nextPokemon.TotalHealth - 1);
                    }

                    nextDonation.Processed = true;
                }

                context.SaveChanges();

                return (nextDonation, pokemon, nextPokemon, trainer, nextTrainer, currentHost, nextHost);
            }
        }

        private static (Pokemon, Trainer, Host) GetNextItems(ExtraLifeContext context, Group activeGroup, PokemonOrder currentPo, Trainer trainer, IEnumerable<PokemonOrder> pokemonOrders, DisplayStatus displayStatus)
        {
            Pokemon nextPokemon = null;
            Trainer nextTrainer = null;
            PokemonOrder nextPo;

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

            displayStatus.CurrentHostId = (displayStatus.CurrentHostId + 1) % (context.Hosts.Count() + 1);

            displayStatus.CurrentHostId = displayStatus.CurrentHostId == 0 ? 1 : displayStatus.CurrentHostId;

            Host nextHost = context.Hosts.First(h => h.Id == displayStatus.CurrentHostId);

            return (nextPokemon, nextTrainer, nextHost);
        }
    }
}