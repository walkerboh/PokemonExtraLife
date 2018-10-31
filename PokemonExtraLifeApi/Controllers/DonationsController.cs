using System.Linq;
using System.Web.Http;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using PokemonExtraLifeApi.EntityFramework;
using PokemonExtraLifeApi.Models.API;

namespace PokemonExtraLifeApi.Controllers
{
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

            return Json(new {donation, currentPokemon, nextPokemon, currentTrainer, nextTrainer, currentHost, nextHost}, settings);
        }

        [HttpGet]
        public IHttpActionResult Games()
        {
            using (ExtraLifeContext context = new ExtraLifeContext())
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
                var displayStatus = context.GetDisplayStatus();
                
                return Json(new
                {
                    numberOfDonations = context.Donations.Count(),
                    totalDonationAmount = context.Donations.Sum(d => d.Amount),
                    donationGoal = displayStatus.DonationGoal
                });
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