using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using PokemonExtraLifeApi.Models.API;

namespace PokemonExtraLifeApi.EntityFramework
{
    [DbConfigurationType(typeof(NpgSqlConfiguration))]
    public class ExtraLifeContext : DbContext
    {
        public ExtraLifeContext() : base("ExtraLifeContext")
        {
            Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Donation> Donations { get; set; }
        public DbSet<Host> Hosts { get; set; }
        public DbSet<Pokemon> Pokemon { get; set; }
        public DbSet<PokemonOrder> PokemonOrders { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<DisplayStatus> DisplayStatus { get; set; }

        public Group ActiveGroup => Groups.Include("PokemonOrders.Pokemon").ToList().FirstOrDefault(g => g.Started && !g.Done);

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.HasDefaultSchema("public");
            base.OnModelCreating(modelBuilder);
        }

        public Gym GetCurrentGym()
        {
            Group group = ActiveGroup;

            if (group != null)
                return group.Gym;

            List<PokemonOrder> pos = PokemonOrders.Include(po => po.Pokemon).Include(po => po.Trainer).ToList();
            PokemonOrder order = pos.FirstOrDefault(po => po.Activated && !po.Done);

            return order?.Trainer.Gym ?? Gym.Blue;
        }

        public DisplayStatus GetDisplayStatus()
        {
            DisplayStatus displayStatus = DisplayStatus.FirstOrDefault();

            if (displayStatus == null)
            {
                displayStatus = new DisplayStatus();
                SaveChanges();
            }

            return displayStatus;
        }
    }
}