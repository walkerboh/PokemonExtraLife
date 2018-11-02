using System;
using System.Collections.Generic;
using System.Linq;
using PokemonExtraLifeApi.Models.API;

namespace PokemonExtraLifeApi.EntityFramework
{
    public static class DonationProcessor
    {
        public static (Donation, Pokemon, Pokemon, Trainer, Trainer, Host, Host) GetNextDonation()
        {
            using (var context = new ExtraLifeContext())
            {
                Donation nextDonation = context.Donations.OrderBy(d => d.Time).FirstOrDefault(d => !d.Processed);

                List<PokemonOrder> pokemonOrders = context.PokemonOrders.Include("Pokemon").Include("Trainer.PokemonOrders.Pokemon").ToList();

                PokemonOrder currentPo = pokemonOrders.FirstOrDefault(po => po.Activated && !po.Done);

                // If we don't have an active PO (first or some weird case) try to find one not in a group and pick it
                if (currentPo == null && pokemonOrders.Any(po => !po.GroupId.HasValue && !po.Activated)) currentPo = pokemonOrders.FirstOrDefault(po => !po.GroupId.HasValue && !po.Activated);

                // if we still don't have a PO, just return the donation
                if (currentPo == null) return (nextDonation, null, null, null, null, null, null);

                DisplayStatus displayStatus = context.GetDisplayStatus();
                Pokemon pokemon = currentPo.Pokemon;
                Trainer trainer = currentPo.Trainer;
                Pokemon nextPokemon = null;
                Trainer nextTrainer = null;
                Host nextHost = null;
                Host currentHost = context.Hosts.First(h => h.Id == displayStatus.CurrentHostId);
                PokemonOrder nextPo = null;

                if (nextDonation != null)
                {
                    decimal overkillDamage = (nextDonation.Amount - pokemon.CurrentHealth) / 2m;
                    pokemon.Damage += nextDonation.Amount;

                    context.SaveChanges();

                    Group activeGroup = context.ActiveGroup;

                    // New pokemon if KOed or group is ended and current PO is in a group
                    if (pokemon.Damage >= pokemon.TotalHealth || currentPo.GroupId.HasValue && activeGroup == null) (nextPokemon, nextTrainer, nextHost, nextPo) = GetNextItems(context, currentPo, trainer, displayStatus);

                    if (nextPokemon?.PokemonOrder?.Trainer != null && !nextPokemon.PokemonOrder.Trainer.Leader && overkillDamage > 0)
                    {
                        while (overkillDamage > 0)
                        {
                            // If there is less overkill damage than next pokemon's health, add damage and set overkill to 0 to break loop
                            // Else, damage the pokemon fully, subtract the damage from the 
                            if (overkillDamage < nextPokemon.CurrentHealth)
                            {
                                nextPokemon.Damage += overkillDamage;
                                overkillDamage = 0;
                            }
                            else
                            {
                                overkillDamage -= nextPokemon.CurrentHealth;
                                nextPokemon.Damage = nextPokemon.CurrentHealth;
                                (nextPokemon, nextTrainer, _, nextPo) = GetNextItems(context, nextPo, trainer, displayStatus);
                            }
                        }
                    }

                    nextDonation.Processed = true;
                }

                context.SaveChanges();

                return (nextDonation, pokemon, nextPokemon, trainer, nextTrainer, currentHost, nextHost);
            }
        }

        private static (Pokemon, Trainer, Host, PokemonOrder) GetNextItems(ExtraLifeContext context, PokemonOrder currentPo, Trainer trainer, DisplayStatus displayStatus)
        {
            List<PokemonOrder> pokemonOrders = context.PokemonOrders.Include("Pokemon").Include("Trainer.PokemonOrders.Pokemon").ToList();
            Group activeGroup = context.ActiveGroup;
            Pokemon nextPokemon = null;
            Trainer nextTrainer = null;
            PokemonOrder nextPo;

            if (activeGroup != null && (currentPo.GroupId == activeGroup.Id || trainer.Done))
            {
                if (!activeGroup.StartTime.HasValue) activeGroup.StartTime = DateTime.Now;

                nextPo = pokemonOrders.Where(po => po.GroupId == activeGroup.Id).OrderBy(po => po.Sequence).FirstOrDefault(po => !po.Activated);
            }
            else
                nextPo = pokemonOrders.Where(po => !po.GroupId.HasValue).OrderBy(po => po.Sequence).FirstOrDefault(po => !po.Activated);

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

            context.SaveChanges();

            return (nextPokemon, nextTrainer, nextHost, nextPo);
        }
    }
}