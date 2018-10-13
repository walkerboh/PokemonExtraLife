using System.Linq;
using System.Web.Mvc;
using PokemonExtraLifeApi.EntityFramework;
using PokemonExtraLifeApi.Models.API;
using PokemonExtraLifeApi.Models.Dashboard;

namespace PokemonExtraLifeApi.Controllers
{
    public class DashboardController : Controller
    {
        public ActionResult Index()
        {
            return View(GetDashboardModel());
        }

        [HttpPost]
        [ActionName("Group")]
        public ActionResult StartGroup(GroupModel model)
        {
            return PartialView("Group", GetGroupModel());
        }

        [HttpDelete]
        [ActionName("Group")]
        public ActionResult StopGroup()
        {
            return PartialView("Group", GetGroupModel());
        }

        private DashboardModel GetDashboardModel()
        {
            return new DashboardModel
            {
                GroupModel = GetGroupModel(),
                SummaryModel = GetSummaryModel()
            };
        }

        private SummaryModel GetSummaryModel()
        {
            using (var context = new ExtraLifeContext())
            {
                var donations = context.Donations.ToList();
                var displayStatus = context.GetDisplayStatus();

                var currentHost = context.Hosts.First(h => h.Id == displayStatus.CurrentHostId);

                var activePokemon = context.PokemonOrders.ToList().FirstOrDefault(po => po.Activated && !po.Done)?.Pokemon;

                return new SummaryModel
                {
                    ActiveHost = currentHost,
                    ActivePokemon = activePokemon,
                    TotalDonations = donations.Count,
                    TotalDonationAmount = donations.Sum(d => d.Amount)
                };
            }
        }

        private GroupModel GetGroupModel()
        {
            using (var context = new ExtraLifeContext())
            {
                var groups = context.Groups.ToList();

                return new GroupModel
                {
                    ActiveGroup = context.ActiveGroup,
                    PotentialGroups = groups.Where(g => !g.Started),
                    PreviouslyActiveGroups = groups.Where(g => g.Started && g.Done)
                };
            }
        }
    }
}