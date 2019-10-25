using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PokemonExtraLifeApiCore.Enum;
using PokemonExtraLifeApiCore.Models.API;

namespace PokemonExtraLifeApiCore.EntityFramework
{
    public class ExtraLifeContext : DbContext
    {
        public ExtraLifeContext(DbContextOptions<ExtraLifeContext> options) :base(options)
        {
        }

        public DbSet<Donation> Donations { get; set; }
        public DbSet<Host> Hosts { get; set; }
        public DbSet<Pokemon> Pokemon { get; set; }
        public DbSet<PokemonOrder> PokemonOrders { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<DisplayStatus> DisplayStatus { get; set; }
        public DbSet<Giveaway> Giveaways { get; set; }
        public DbSet<PopupPrize> Prizes { get; set; }
        public DbSet<Fact> Facts { get; set; }

        public Group ActiveGroup => Groups.Include("PokemonOrders.Pokemon").ToList().FirstOrDefault(g => g.Started && !g.Done);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            base.OnModelCreating(modelBuilder);
        }

        public Gym? GetCurrentGym()
        {
            Group group = ActiveGroup;

            if (group != null)
                return group.Gym;

            List<PokemonOrder> pos = PokemonOrders.Include(po => po.Pokemon).Include(po => po.Trainer).ToList();
            PokemonOrder order = pos.FirstOrDefault(po => po.Activated && !po.Done);

            return order?.Trainer.Gym;
        }

        public int? GetCurrentPrizeId()
        {
            return Prizes.SingleOrDefault(p => p.Active())?.Id;
        }

        public DisplayStatus GetDisplayStatus()
        {
            DisplayStatus displayStatus = DisplayStatus.FirstOrDefault();

            if (displayStatus == null)
            {
                displayStatus = new DisplayStatus
                {
                    CurrentHostId = Hosts.First().Id,
                    HealthMultiplier = 1
                };
                DisplayStatus.Add(displayStatus);
                SaveChanges();
            }

            return displayStatus;
        }
    }
}