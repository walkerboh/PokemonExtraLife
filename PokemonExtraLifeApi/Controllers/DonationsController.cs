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
        private JsonSerializerSettings settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        
        [HttpGet]
        public IHttpActionResult Donations()
        {
            using (ExtraLifeContext context = new ExtraLifeContext())
                return Json(context.Donations.ToList());
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
            (Donation donation, Pokemon nextPokemon, Trainer nextTrainer, Host nextHost) = DonationProcessor.GetNextDonation();

            return Json(new {donation, nextPokemon, nextTrainer, nextHost}, settings);
        }

        [HttpGet]
        public IHttpActionResult Games()
        {
            using (var context = new ExtraLifeContext())
            {
                var displayStatus = context.GetDisplayStatus();

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