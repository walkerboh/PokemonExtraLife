using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using PokemonExtraLifeApi.EntityFramework;
using PokemonExtraLifeApi.Models.API;

namespace PokemonExtraLifeApi.Controllers
{
    public class DonationsController : ApiController
    {
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

            return Json(new {donation, nextPokemon, nextTrainer, nextHost});
        }
    }
}