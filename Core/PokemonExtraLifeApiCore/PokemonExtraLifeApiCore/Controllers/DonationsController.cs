using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PokemonExtraLifeApiCore.EntityFramework;
using PokemonExtraLifeApiCore.Enum;
using PokemonExtraLifeApiCore.Models.API;

namespace PokemonExtraLifeApiCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonationsController : Controller
    {
        private readonly JsonSerializerSettings settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        private readonly ExtraLifeContext _context;
        private readonly DonationProcessor _donationProcessor;

        public DonationsController(ExtraLifeContext context, DonationProcessor processor)
        {
            _context = context;
            _donationProcessor = processor;
        }

        [HttpGet]
        [Route("nextdonation")]
        public ActionResult NextDonation()
        {
            (Donation donation, Pokemon currentPokemon, Pokemon nextPokemon, Trainer currentTrainer, Trainer nextTrainer, Host currentHost, Host nextHost) = _donationProcessor.GetNextDonation();

            var done = _context.Trainers.Include("PokemonOrders.Pokemon").ToList().All(t => t.PokemonOrders.All(po => po.Done));

            return Json(new {donation, currentPokemon, nextPokemon, currentTrainer, nextTrainer, currentHost, nextHost, done}, settings);
        }

        [HttpGet]
        [Route("currentstatus")]
        public ActionResult CurrentStatus()
        {
            List<PokemonOrder> pokemonOrders = _context.PokemonOrders.Include("Pokemon").Include("Trainer").ToList();

            PokemonOrder currentPo = pokemonOrders.FirstOrDefault(po => po.Activated && !po.Done);

            bool done = pokemonOrders.All(po => po.Done);

            return Json(new { currentPo?.Trainer, currentPo?.Pokemon, done });
        }

        [HttpGet]
        [Route("games")]
        public ActionResult Games()
        {
            DisplayStatus displayStatus = _context.GetDisplayStatus();

            return Json(new
            {
                displayStatus.CurrentGame,
                displayStatus.NextGame,
                displayStatus.FollowingGame
            }, settings);
        }

        [HttpGet]
        [Route("summary")]
        public ActionResult Summary()
        {
            DisplayStatus displayStatus = _context.GetDisplayStatus();

            return Json(new
            {
                numberOfDonations = _context.Donations.Count(),
                totalDonationAmount = _context.Donations.Sum(d => d.Amount),
                donationGoal = displayStatus.DonationGoal
            });
        }

        [HttpGet]
        [Route("giveaways")]
        public ActionResult Giveaways()
        {
            Gym? currentGym = _context.GetCurrentGym();

            Giveaway giveaway = currentGym.HasValue ? _context.Giveaways.ToList().Single(g => g.Gym.Equals(currentGym.Value)) : null;

            return Json(new
            {
                giveaway
            });
        }

        [HttpGet]
        [Route("gymstatus")]
        public ActionResult GymStatus()
        {
            var activeGyms = new List<Gym> { Gym.Flying, Gym.Bug, Gym.Normal, Gym.Ghost, Gym.Fighting, Gym.Steel, Gym.Ice, Gym.Dragon};

            var trainers = _context.Trainers.Include("PokemonOrders.Pokemon").Where(t => t.Gym.HasValue).ToList();

            var gyms = from trainer in trainers
                       where activeGyms.Contains(trainer.Gym.Value)
                       group trainer by trainer.Gym
                       into grp
                       select new
                       {
                           gym = grp.Key,
                           gymName = grp.Key.ToString(),
                           trainerName = grp.First().Name,
                           started = grp.Any(t => t.PokemonOrders.Any(po => po.Activated)),
                           done = grp.All(t => t.PokemonOrders.All(po => po.Done))
                       };

            return Json(gyms);
        }

        [HttpGet]
        [Route("prize")]
        public ActionResult Prize()
        {
            var prizeId = _context.GetCurrentPrizeId();

            if (!prizeId.HasValue)
            {
                return Json(null);
            }

            var activePrize = _context.Prizes.Find(prizeId.Value);

            return Json(activePrize);
        }

        [HttpGet]
        [Route("reset")]
        public async Task<ActionResult> Reset()
        {
            await _context.Donations.ForEachAsync(d => d.Processed = false);
            await _context.Pokemon.ForEachAsync(p => p.Damage = 0);
            await _context.Groups.ForEachAsync(g =>
            {
                g.Started = false;
                g.StartTime = null;
            });
            _context.GetDisplayStatus().CurrentHostId = 1;
            await _context.PokemonOrders.ForEachAsync(po => po.Activated = false);
            _context.PokemonOrders.OrderBy(po=>po.Sequence).First().Activated = true;
            await _context.Prizes.ForEachAsync(p =>
            {
                p.StartTime = null;
                p.Duration = null;
                p.WiningDonor = null;
            });

            _context.SaveChanges();

            return Json("OK");
        }
    }
}