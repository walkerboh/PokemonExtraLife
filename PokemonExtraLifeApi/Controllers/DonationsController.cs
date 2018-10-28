using System.Data.Entity;
using System.Linq;
using System.Web.Http;
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
        public IHttpActionResult Donations()
        {
            using (ExtraLifeContext context = new ExtraLifeContext())
            {
                return Json(context.Donations.ToList());
            }
        }

        [HttpGet]
        public IHttpActionResult Hosts()
        {
            using (ExtraLifeContext context = new ExtraLifeContext())
            {
                return Json(context.Hosts.Include(h => h.Pokemon).ToList());
            }
        }

        [HttpGet]
        public IHttpActionResult NextDonation()
        {
            (Donation donation, Pokemon currentPokemon, Pokemon nextPokemon, Trainer nextTrainer, Host nextHost) = DonationProcessor.GetNextDonation();

            return Json(new {donation, currentPokemon, nextPokemon, nextTrainer, nextHost}, settings);
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
                });
            }
        }
    }
}