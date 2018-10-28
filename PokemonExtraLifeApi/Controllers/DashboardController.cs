using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
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
        public ActionResult UpdateSummary(SummaryModel model)
        {
            UpdateDisplayStatus(model);

            return PartialView("Summary", GetSummaryModel());
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
                GamesModel = GetGamesModel(),
                DonationsModel = GetDonationsModel(),
                PokemonStatusModel = GetPokemonStatusModel()
            };
        }

        private SummaryModel GetSummaryModel()
        {
            using (ExtraLifeContext context = new ExtraLifeContext())
            {
                List<Donation> donations = context.Donations.ToList();
                DisplayStatus displayStatus = context.GetDisplayStatus();

                Host currentHost = context.Hosts.First(h => h.Id == displayStatus.CurrentHostId);

                Pokemon activePokemon = context.PokemonOrders.Include(po => po.Pokemon).ToList().FirstOrDefault(po => po.Activated && !po.Done)?.Pokemon;

                return new SummaryModel
                {
                    ActiveHost = currentHost,
                    ActivePokemon = activePokemon,
                    TotalDonations = donations.Count,
                    TotalDonationAmount = donations.Sum(d => d.Amount),
                    DonationGoal = displayStatus.DonationGoal,
                    TrackDonations = displayStatus.TrackDonations,
                    HealthMultiplier = displayStatus.HealthMultiplier
                };
            }
        }

        private void UpdateDisplayStatus(SummaryModel model)
        {
            using (ExtraLifeContext context = new ExtraLifeContext())
            {
                DisplayStatus displayStatus = context.GetDisplayStatus();

                displayStatus.TrackDonations = model.TrackDonations;
                displayStatus.DonationGoal = model.DonationGoal;

                if (displayStatus.HealthMultiplier != model.HealthMultiplier)
                {
                    displayStatus.HealthMultiplier = model.HealthMultiplier;
                    context.Pokemon.Include(p=>p.PokemonOrder).Where(p=>!p.PokemonOrder.Activated).ForEach(p=>p.HealthMultiplier = model.HealthMultiplier);
                }

                context.SaveChanges();
            }
        }

        private GroupModel GetGroupModel()
        {
            using (ExtraLifeContext context = new ExtraLifeContext())
            {
                List<Group> groups = context.Groups.ToList();

                return new GroupModel
                {
                    ActiveGroup = context.ActiveGroup,
                    PotentialGroups = groups.Where(g => !g.Started),
                    PreviouslyActiveGroups = groups.Where(g => g.Started && g.Done)
                };
            }
        }

        private void ActivateGroup(int groupId)
        {
            using (ExtraLifeContext context = new ExtraLifeContext())
            {
                Group group = context.Groups.First(g => g.Id == groupId);
                group.Started = true;
                context.SaveChanges();
            }
        }

        private void ForceStopGroup()
        {
            using (ExtraLifeContext context = new ExtraLifeContext())
            {
                IQueryable<Group> groups = context.Groups.Where(g => g.Started).Include("PokemonOrders.Pokemon");

                foreach (Group group in groups)
                {
                    if (!group.Done) group.ForceComplete = true;
                }

                context.SaveChanges();
            }
        }

        private GamesModel GetGamesModel()
        {
            using (ExtraLifeContext context = new ExtraLifeContext())
            {
                DisplayStatus displayStatus = context.GetDisplayStatus();

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
            using (ExtraLifeContext context = new ExtraLifeContext())
            {
                DisplayStatus displayStatus = context.GetDisplayStatus();

                displayStatus.CurrentGame = model.CurrentGame;
                displayStatus.NextGame = model.NextGame;
                displayStatus.FollowingGame = model.FollowingGame;

                context.SaveChanges();
            }
        }

        private DonationsModel GetDonationsModel()
        {
            using (ExtraLifeContext context = new ExtraLifeContext())
            {
                return new DonationsModel
                {
                    Donations = context.Donations.OrderByDescending(d => d.Time).ToList()
                };
            }
        }

        private PokemonStatusModel GetPokemonStatusModel()
        {
            using (ExtraLifeContext context = new ExtraLifeContext())
            {
                PokemonOrder currentPo = context.PokemonOrders.Include("Pokemon").Include("Trainer").ToList().FirstOrDefault(po => po.Activated && !po.Done);

                if (currentPo != null)
                {
                    return new PokemonStatusModel
                    {
                        CurrentPokemon = currentPo.Pokemon,
                        CurrentTrainer = currentPo.Trainer,
                        CurrentGym = currentPo.Trainer.Gym
                    };
                }

                return new PokemonStatusModel();
            }
        }
    }
}