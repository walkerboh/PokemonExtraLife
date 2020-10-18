using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PokemonExtraLifeApiCore.EntityFramework;
using PokemonExtraLifeApiCore.Models.Dashboard;
using System;
using System.Linq;

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

        [HttpPost]
        public ActionResult Players(PlayersModel model)
        {
            UpdatePlayers(model);

            return PartialView("Players", GetPlayersModel());
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
                PrizesModel = GetPrizesModel(),
                PlayersModel = GetPlayersModel()
            };
        }

        private SummaryModel GetSummaryModel()
        {
            var donations = context.Donations.ToList();
            var displayStatus = context.GetDisplayStatus();

            var currentHost = context.Hosts.FirstOrDefault(h => h.Id == displayStatus.CurrentHostId);

            var activePokemon = context.PokemonOrders.Include(po => po.Pokemon).ToList()
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
            var displayStatus = context.GetDisplayStatus();

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
            var groups = context.Groups.ToList();

            return new GroupModel
            {
                ActiveGroup = context.ActiveGroup,
                PotentialGroups = groups.Where(g => !g.Started),
                PreviouslyActiveGroups = groups.Where(g => g.Started && g.Done)
            };
        }

        private void ActivateGroup(int groupId)
        {
            var group = context.Groups.First(g => g.Id == groupId);
            group.Started = true;
            context.SaveChanges();
        }

        private void ForceStopGroup()
        {
            var groups = context.Groups.Where(g => g.Started).Include("PokemonOrders.Pokemon");

            foreach (var group in groups)
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
            var displayStatus = context.GetDisplayStatus();

            return new GamesModel
            {
                CurrentGame = displayStatus.CurrentGame,
                NextGame = displayStatus.NextGame,
                FollowingGame = displayStatus.FollowingGame
            };
        }

        private void UpdateGamesDb(GamesModel model)
        {
            var displayStatus = context.GetDisplayStatus();

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
            var currentPo = context.PokemonOrders.Include("Pokemon").Include("Trainer").ToList()
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

            if (names.Count == 0)
            {
                prize.WiningDonor = "No donations for prize";
                context.SaveChanges();
                return;
            }

            var index = _random.Next(names.Count);
            prize.WiningDonor = names[index];
            context.SaveChanges();
        }

        private PlayersModel GetPlayersModel()
        {
            return new PlayersModel
            {
                Players = context.Players.OrderBy(p => p.Id).ToList()
            };
        }

        private void UpdatePlayers(PlayersModel model)
        {
            foreach (var player in model.Players)
            {
                var dbPlayer = context.Players.Single(p => p.Id == player.Id);
                if (!string.IsNullOrEmpty(player.Name) && !player.Name.Equals(dbPlayer.Name))
                {
                    dbPlayer.Name = player.Name;
                }
                if (!string.IsNullOrEmpty(player.Color) && !player.Color.Equals(dbPlayer.Color))
                {
                    dbPlayer.Color = player.Color;
                }
            }

            context.SaveChanges();
        }

        //private PlayersModel GetPlayersModel()
        //{
        //    var players = context.Players.ToList();

        //    return new PlayersModel
        //    {
        //        Player1 = players.SingleOrDefault(p => p.Id == 1),
        //        Player2 = players.SingleOrDefault(p => p.Id == 2),
        //        Player3 = players.SingleOrDefault(p => p.Id == 3),
        //        Player4 = players.SingleOrDefault(p => p.Id == 4),
        //        Player5 = players.SingleOrDefault(p => p.Id == 5),
        //        Player6 = players.SingleOrDefault(p => p.Id == 6),
        //        Player7 = players.SingleOrDefault(p => p.Id == 7),
        //        Player8 = players.SingleOrDefault(p => p.Id == 8),
        //        Player9 = players.SingleOrDefault(p => p.Id == 9),
        //        Player10 = players.SingleOrDefault(p => p.Id == 10)
        //    };
        //}

        //private void UpdatePlayers(PlayersModel model)
        //{
        //    var players = model.AsList;

        //    foreach(var player in players)
        //    {
        //        var dbPlayer = context.Players.Single(p => p.Id == player.Id);
        //        if(!player.Name.Equals(dbPlayer.Name))
        //        {
        //            dbPlayer.Name = player.Name;
        //        }
        //        if(!player.Color.Equals(dbPlayer.Color))
        //        {
        //            dbPlayer.Color = player.Color;
        //        }
        //    }

        //    context.SaveChanges();
        //}
    }
}