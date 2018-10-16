using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
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
            if (!model.SelectedGroup.HasValue)
                return PartialView("Group", GetGroupModel());

            ActivateGroup(model.SelectedGroup.Value);
            
            return PartialView("Group", GetGroupModel());
        }

        [HttpDelete]
        [ActionName("Group")]
        public ActionResult StopGroup()
        {
            ForceStopGroup();
            
            return PartialView("Group", GetGroupModel());
        }

        [HttpPost]
        public ActionResult UpdateGames(GamesModel model)
        {
            UpdateGamesDb(model);

            return PartialView("Games", GetGamesModel());
        }

        private DashboardModel GetDashboardModel()
        {
            return new DashboardModel
            {
                GroupModel = GetGroupModel(),
                SummaryModel = GetSummaryModel(),
                GamesModel = GetGamesModel()
            };
        }

        private SummaryModel GetSummaryModel()
        {
            using (var context = new ExtraLifeContext())
            {
                var donations = context.Donations.ToList();
                var displayStatus = context.GetDisplayStatus();

                var currentHost = context.Hosts.First(h => h.Id == displayStatus.CurrentHostId);

                var activePokemon = context.PokemonOrders.Include(po => po.Pokemon).ToList().FirstOrDefault(po => po.Activated && !po.Done)?.Pokemon;

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
        
        private void ActivateGroup(int group)
        {
            using (var context = new ExtraLifeContext())
            {
                context.Groups.First(g => g.Id == group).Started = true;
                context.SaveChanges();
            }
        }

        private void ForceStopGroup()
        {
            using (var context = new ExtraLifeContext())
            {
                var groups = context.Groups.Where(g => g.Started);

                foreach (var group in groups)
                {
                    if (!group.Done)
                    {
                        group.ForceComplete = true;
                    }
                }

                context.SaveChanges();
            }
        }

        private GamesModel GetGamesModel()
        {
            using (var context = new ExtraLifeContext())
            {
                var displayStatus = context.GetDisplayStatus();
                
                return new GamesModel
                {
                    CurrentGame = displayStatus.CurrentGame,
                    NextGame = displayStatus.NextGame,
                    FollowingGame = displayStatus.FollowingGame
                };
            }
        }

        private void UpdateGamesDb(GamesModel model)
        {
            using (var context = new ExtraLifeContext())
            {
                var displayStatus = context.GetDisplayStatus();

                displayStatus.CurrentGame = model.CurrentGame;
                displayStatus.NextGame = model.NextGame;
                displayStatus.FollowingGame = model.FollowingGame;

                context.SaveChanges();
            }
        }
    }
}