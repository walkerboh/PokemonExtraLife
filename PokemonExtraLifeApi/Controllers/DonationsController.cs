using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc.Html;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using PokemonExtraLifeApi.Common;
using PokemonExtraLifeApi.EntityFramework;
using PokemonExtraLifeApi.Models.API;

namespace PokemonExtraLifeApi.Controllers
{
    [AllowCrossSite]
    public class DonationsController : ApiController
    {
        private readonly JsonSerializerSettings settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        [HttpGet]
        public IHttpActionResult NextDonation()
        {
            (Donation donation, Pokemon currentPokemon, Pokemon nextPokemon, Trainer currentTrainer, Trainer nextTrainer, Host currentHost, Host nextHost) = DonationProcessor.GetNextDonation();

            bool done;
            
            using (var context = new ExtraLifeContext())
            {
                done = context.Trainers.Include("PokemonOrders.Pokemon").ToList().All(t => t.PokemonOrders.All(po => po.Done));
            }

            return Json(new { donation, currentPokemon, nextPokemon, currentTrainer, nextTrainer, currentHost, nextHost, done }, settings);
        }

        [HttpGet]
        public IHttpActionResult CurrentStatus()
        {
            using (var context = new ExtraLifeContext())
            {
                List<PokemonOrder> pokemonOrders = context.PokemonOrders.Include("Pokemon").Include("Trainer").ToList();
                
                PokemonOrder currentPo = pokemonOrders.FirstOrDefault(po => po.Activated && !po.Done);

                bool done = pokemonOrders.All(po => po.Done);

                return Json(new { currentPo?.Trainer, currentPo?.Pokemon, done });
            }
        }

        [HttpGet]
        public IHttpActionResult Games()
        {
            using (var context = new ExtraLifeContext())
            {
                DisplayStatus displayStatus = context.GetDisplayStatus();

                return Json(new
                {
                    displayStatus.CurrentGame,
                    displayStatus.NextGame,
                    displayStatus.FollowingGame
                }, settings);
            }
        }

        [HttpGet]
        public IHttpActionResult Summary()
        {
            using (var context = new ExtraLifeContext())
            {
                DisplayStatus displayStatus = context.GetDisplayStatus();

                return Json(new
                {
                    numberOfDonations = context.Donations.Count(),
                    totalDonationAmount = context.Donations.Sum(d => d.Amount),
                    donationGoal = displayStatus.DonationGoal
                });
            }
        }

        [HttpGet]
        public IHttpActionResult Giveaways()
        {
            using (var context = new ExtraLifeContext())
            {
                Gym? currentGym = context.GetCurrentGym();

                Giveaway giveaway = currentGym.HasValue ? context.Giveaways.ToList().Single(g => g.Gym.Equals(currentGym.Value)) : null;

                return Json(new
                {
                    giveaway
                });
            }
        }

        [HttpGet]
        public IHttpActionResult GymStatus()
        {
            var activeGyms = new List<Gym> { Gym.Rock, Gym.Water, Gym.Electric, Gym.Grass, Gym.Poison, Gym.Psychic, Gym.Fire, Gym.Ground };
            
            using (var context = new ExtraLifeContext())
            {
                var trainers = context.Trainers.Include("PokemonOrders.Pokemon").Where(t => t.Gym.HasValue).ToList();

                var gyms = from trainer in trainers
                           where activeGyms.Contains(trainer.Gym.Value) 
                           group trainer by trainer.Gym
                           into grp
                           select new
                           {
                               gym = grp.Key,
                               gymName = grp.Key.ToString(),
                               started = grp.Any(t => t.PokemonOrders.Any(po => po.Activated)),
                               done = grp.All(t => t.PokemonOrders.All(po => po.Done))
                           };

                return Json(new { grouped = gyms });
            }
        }

        [HttpGet]
        public IHttpActionResult Reset()
        {
            using (var context = new ExtraLifeContext())
            {
                context.Donations.ForEach(d => d.Processed = false);
                context.Pokemon.ForEach(p => p.Damage = 0);
                context.Groups.ForEach(g =>
                {
                    g.Started = false;
                    g.StartTime = null;
                });
                context.GetDisplayStatus().CurrentHostId = 1;
                context.PokemonOrders.ForEach(po => po.Activated = false);
                context.PokemonOrders.First().Activated = true;

                context.SaveChanges();
            }

            return Json("OK");
        }
    }
}