using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PokemonExtraLifeApiCore.Models.API;

namespace PokemonExtraLifeApiCore.EntityFramework
{
    public class PokemonDonationProcessor : IDonationProcessor
    {
        private readonly ExtraLifeContext _context;

        public PokemonDonationProcessor(ExtraLifeContext context)
        {
            _context = context;
        }

        public IDonationDisplayModel GetNextDonation()
        {
            var nextDonation = _context.Donations.OrderBy(d => d.Time).FirstOrDefault(d => !d.Processed);

            var pokemonOrders = _context.PokemonOrders.Include("Pokemon").Include("Trainer.PokemonOrders.Pokemon").ToList();

            var currentPo = pokemonOrders.FirstOrDefault(po => po.Activated && !po.Done);

            // If we don't have an active PO (first or some weird case) try to find one not in a group and pick it
            if (currentPo == null && pokemonOrders.Any(po => !po.GroupId.HasValue && !po.Activated))
            {
                currentPo = pokemonOrders.FirstOrDefault(po => !po.GroupId.HasValue && !po.Activated);
            }

            // if we still don't have a PO, just return the donation
            if (currentPo == null)
                return new PokemonDonationDisplayModel
                {
                    Donation = nextDonation,
                    Done = _context.Trainers.Include("PokemonOrders.Pokemon").ToList().All(t => t.PokemonOrders.All(po => po.Done))
                };

            var displayStatus = _context.GetDisplayStatus();
            var pokemon = currentPo.Pokemon;
            var trainer = currentPo.Trainer;
            Pokemon nextPokemon = null;
            Trainer nextTrainer = null;
            Host nextHost = null;
            var currentHost = _context.Hosts.First(h => h.Id == displayStatus.CurrentHostId);
            PokemonOrder nextPo = null;

            if (nextDonation != null)
            {
                var overkillDamage = (nextDonation.Amount - pokemon.CurrentHealth);
                pokemon.Damage += nextDonation.Amount;

                _context.SaveChanges();

                var activeGroup = _context.ActiveGroup;

                // New pokemon if KOed or group is ended and current PO is in a group
                if (pokemon.Damage >= pokemon.TotalHealth || currentPo.GroupId.HasValue && activeGroup == null) (nextPokemon, nextTrainer, nextHost, nextPo) = GetNextItems(_context, currentPo, trainer, displayStatus);

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
                            (nextPokemon, nextTrainer, _, nextPo) = GetNextItems(_context, nextPo, trainer, displayStatus);
                        }
                    }
                }

                nextDonation.Processed = true;
            }

            _context.SaveChanges();

            return new PokemonDonationDisplayModel
            {
                Donation = nextDonation,
                CurrentPokemon = pokemon,
                NextPokemon = nextPokemon,
                CurrentTrainer = trainer,
                NextTrainer = nextTrainer,
                CurrentHost = currentHost,
                NextHost = nextHost,
                Done = _context.Trainers.Include("PokemonOrders.Pokemon").ToList().All(t => t.PokemonOrders.All(po => po.Done))
        };
        }

        private static (Pokemon, Trainer, Host, PokemonOrder) GetNextItems(ExtraLifeContext context, PokemonOrder currentPo, Trainer trainer, DisplayStatus displayStatus)
        {
            var pokemonOrders = context.PokemonOrders.Include("Pokemon").Include("Trainer.PokemonOrders.Pokemon").ToList();
            var activeGroup = context.ActiveGroup;
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

            var nextHost = context.Hosts.First(h => h.Id == displayStatus.CurrentHostId);

            context.SaveChanges();

            return (nextPokemon, nextTrainer, nextHost, nextPo);
        }
    }
}