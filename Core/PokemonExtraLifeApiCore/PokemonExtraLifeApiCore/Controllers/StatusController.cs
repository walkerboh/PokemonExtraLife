using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PokemonExtraLifeApiCore.EntityFramework;
using PokemonExtraLifeApiCore.Models.API;

namespace PokemonExtraLifeApiCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : Controller
    {
        private readonly ExtraLifeContext _context;

        public StatusController(ExtraLifeContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("giveaways")]
        public ActionResult Giveaways()
        {
            var trainers = _context.Trainers.Include("PokemonOrders.Pokemon").Where(t => t.Gym.HasValue).ToList();
            var giveaways = _context.Giveaways.ToList();

            var gyms = from trainer in trainers
                       group trainer by trainer.Gym
                into grp
                       join giveaway in giveaways on grp.Key equals giveaway.Gym
                       select new
                       {
                           gym = grp.Key,
                           gymName = grp.Key.ToString(),
                           trainer = grp.First().Name,
                           started = grp.Any(t => t.PokemonOrders.Any(po => po.Activated)),
                           done = grp.All(t => t.PokemonOrders.All(po => po.Done)),
                           giveaway
                       };

            return Json(new { giveaways = gyms });
        }

        [HttpGet]
        [Route("donations")]
        public ActionResult Donations()
        {
            var donations = _context.Donations.ToList();

            var numberOfDonations = donations.Count;
            var totalAmountDonated = donations.Sum(d => d.Amount);
            var latestDonation = donations.OrderByDescending(d => d.Time).First();
            var donationGoal = _context.GetDisplayStatus().DonationGoal;

            return Json(new {numberOfDonations, totalAmountDonated, latestDonation, donationGoal});
        }

        [HttpGet]
        [Route("Pokemon")]
        public ActionResult Pokemon()
        {
            var pokemonOrders = _context.PokemonOrders.Include("Pokemon").Include("Trainer").ToList();

            var currentPo = pokemonOrders.FirstOrDefault(po => po.Activated && !po.Done);

            var gym = _context.GetCurrentGym();

            return Json(new {currentPo?.Trainer, currentPo?.Pokemon, gym = gym?.ToString()});
        }
    }
}