using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Mvc;
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

        [System.Web.Http.HttpGet]
        public IHttpActionResult NextDonation()
        {
            (Donation donation, Pokemon currentPokemon, Pokemon nextPokemon, Trainer currentTrainer, Trainer nextTrainer, Host currentHost, Host nextHost) = DonationProcessor.GetNextDonation();

            return Json(new {donation, currentPokemon, nextPokemon, currentTrainer, nextTrainer, currentHost, nextHost}, settings);
        }

        [System.Web.Http.HttpGet]
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

        [System.Web.Http.HttpGet]
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