using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PokemonExtraLifeApiCore.EntityFramework;
using PokemonExtraLifeApiCore.Models.API;
using PokemonExtraLifeApiCore.Models.Dashboard;

namespace PokemonExtraLifeApiCore.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ExtraLifeContext context;
        private readonly Random _random;

        public DashboardController(ExtraLifeContext context, Random random)
        {
            this.context = context;
            this._random = random;
        }

        public ActionResult Index()
        {
            return View(GetDashboardModel());
        }

        [HttpPost]
        public ActionResult StartGroup(GroupModel groupModel)
        {
            if (!groupModel.SelectedGroup.HasValue)
                return PartialView("Group", GetGroupModel());

            ActivateGroup(groupModel.SelectedGroup.Value);

            return PartialView("Group", GetGroupModel());
        }

        [HttpPost]
        public ActionResult StopGroup()
        {
            ForceStopGroup();

            return PartialView("Group", GetGroupModel());
        }

        [HttpPost]
        public ActionResult UpdateSummary(SummaryModel summaryModel)
        {
            UpdateDisplayStatus(summaryModel);

            return PartialView("Summary", GetSummaryModel());
        }

        [HttpPost]
        public ActionResult UpdateGames(GamesModel gamesModel)
        {
            UpdateGamesDb(gamesModel);

            return PartialView("Games", GetGamesModel());
        }

        [HttpPost]
        public ActionResult StartPrize(int prizeId)
        {
            if (context.GetCurrentPrizeId() != null)
                return PartialView("Prizes", GetPrizesModel());

            ActivatePrize(prizeId);

            return PartialView("Prizes", GetPrizesModel());
        }

        [HttpPost]
        public ActionResult StopPrize()
        {
            var prizeId = context.GetCurrentPrizeId();

            if (!prizeId.HasValue)
                return PartialView("Prizes", GetPrizesModel());

            StopPrize(prizeId.Value);

            return PartialView("Prizes", GetPrizesModel());
        }

        [HttpPost]
        public ActionResult PickWinner(int prizeId)
        {
            UpdateWinner(prizeId);

            return PartialView("Prizes", GetPrizesModel());
        }

        private DashboardModel GetDashboardModel()
        {
            return new DashboardModel
            {
                GroupModel = GetGroupModel(),
                SummaryModel = GetSummaryModel(),
                GamesModel = GetGamesModel(),
                DonationsModel = GetDonationsModel(),
                PokemonStatusModel = GetPokemonStatusModel(),
                PrizesModel = GetPrizesModel()
            };
        }

        private SummaryModel GetSummaryModel()
        {
            List<Donation> donations = context.Donations.ToList();
            DisplayStatus displayStatus = context.GetDisplayStatus();

            Host currentHost = context.Hosts.First(h => h.Id == displayStatus.CurrentHostId);

            Pokemon activePokemon = context.PokemonOrders.Include(po => po.Pokemon).ToList()
                .FirstOrDefault(po => po.Activated && !po.Done)?.Pokemon;

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

        private void UpdateDisplayStatus(SummaryModel model)
        {
            DisplayStatus displayStatus = context.GetDisplayStatus();

            displayStatus.TrackDonations = model.TrackDonations;
            displayStatus.DonationGoal = model.DonationGoal;

            if (displayStatus.HealthMultiplier != model.HealthMultiplier)
            {
                displayStatus.HealthMultiplier = model.HealthMultiplier;
                foreach (var p in context.Pokemon.Include(p => p.PokemonOrder)
                    .Where(p => !p.PokemonOrder.Activated))
                {
                    p.HealthMultiplier = model.HealthMultiplier;
                }
            }

            context.SaveChanges();
        }

        private GroupModel GetGroupModel()
        {
            List<Group> groups = context.Groups.ToList();

            return new GroupModel
            {
                ActiveGroup = context.ActiveGroup,
                PotentialGroups = groups.Where(g => !g.Started),
                PreviouslyActiveGroups = groups.Where(g => g.Started && g.Done)
            };
        }

        private void ActivateGroup(int groupId)
        {
            Group group = context.Groups.First(g => g.Id == groupId);
            group.Started = true;
            context.SaveChanges();
        }

        private void ForceStopGroup()
        {
            IQueryable<Group> groups = context.Groups.Where(g => g.Started).Include("PokemonOrders.Pokemon");

            foreach (Group group in groups)
            {
                if (!group.Done)
                    group.ForceComplete = true;
            }

            context.SaveChanges();
        }

        private void ActivatePrize(int prizeId)
        {
            var prize = context.Prizes.Find(prizeId);
            prize.StartTime = DateTime.UtcNow;
            context.SaveChanges();
        }

        private void StopPrize(int prizeId)
        {
            var prize = context.Prizes.Find(prizeId);
            prize.EndTime = DateTime.UtcNow;
            context.SaveChanges();
        }

        private GamesModel GetGamesModel()
        {
            DisplayStatus displayStatus = context.GetDisplayStatus();

            return new GamesModel
            {
                CurrentGame = displayStatus.CurrentGame,
                NextGame = displayStatus.NextGame,
                FollowingGame = displayStatus.FollowingGame
            };
        }

        private void UpdateGamesDb(GamesModel model)
        {
            DisplayStatus displayStatus = context.GetDisplayStatus();

            displayStatus.CurrentGame = model.CurrentGame;
            displayStatus.NextGame = model.NextGame;
            displayStatus.FollowingGame = model.FollowingGame;

            context.SaveChanges();
        }

        private DonationsModel GetDonationsModel()
        {
            return new DonationsModel
            {
                Donations = context.Donations.OrderByDescending(d => d.Time).ToList()
            };
        }

        private PokemonStatusModel GetPokemonStatusModel()
        {
            PokemonOrder currentPo = context.PokemonOrders.Include("Pokemon").Include("Trainer").ToList()
                .FirstOrDefault(po => po.Activated && !po.Done);

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

        private PrizesModel GetPrizesModel()
        {
            var model = new PrizesModel
            {
                Prizes = context.Prizes.OrderBy(p => p.Id).ToList(),
                ActivePrizeId = context.GetCurrentPrizeId()
            };

            if (model.ActivePrizeId.HasValue)
            {
                model.ActivePrizeDonations = context.Donations.Count(d => d.PrizeId.Equals(model.ActivePrizeId));
            }

            return model;
        }

        private void UpdateWinner(int prizeId)
        {
            var prize = context.Prizes.Find(prizeId);
            if (!string.IsNullOrEmpty(prize.WiningDonor))
            {
                return;
            }

            var donations = context.Donations.Where(d => d.PrizeId.Equals(prize.Id));
            var names = donations.Select(d => d.Donor).Distinct().ToList();
            
            if(names.Count == 0)
            {
                prize.WiningDonor = "No donations for prize";
                context.SaveChanges();
                return;
            }
            
            var index = _random.Next(names.Count);
            prize.WiningDonor = names[index];
            context.SaveChanges();
        }
    }
}