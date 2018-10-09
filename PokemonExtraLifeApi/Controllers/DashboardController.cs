using System.Linq;
using System.Web.Mvc;
using PokemonExtraLifeApi.EntityFramework;
using PokemonExtraLifeApi.Models.Dashboard;

namespace PokemonExtraLifeApi.Controllers
{
    public class DashboardController : Controller
    {
        public ActionResult Index()
        {
            return null;
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

                var currentHost = context.Hosts.First(h => h.Id == context.GetDisplayStatus().CurrentHostId);

                var activePokemon = context.PokemonOrders.FirstOrDefault(po => po.Activated && !po.Done)?.Pokemon;

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