using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using PokemonExtraLifeApi.Models.API;

namespace PokemonExtraLifeApi.EntityFramework
{
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
        private DbSet<DisplayStatus> DisplayStatus { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public Group ActiveGroup => Groups.Include(g => g.PokemonOrders).FirstOrDefault(g => g.Started && !g.Done);

        public Gym GetCurrentGym()
        {
            var group = ActiveGroup;

            if (group != null)
                return group.Gym;

            var pos = PokemonOrders.Include(po => po.Pokemon).Include(po => po.Trainer).ToList();
            return pos.First(po => !po.Done).Trainer.Gym;
        }

        public DisplayStatus GetDisplayStatus()
        {
            var displayStatus = DisplayStatus.FirstOrDefault();

            if (displayStatus == null)
            {
                displayStatus = new DisplayStatus();
                SaveChanges();
            }

            return displayStatus;
        }
    }
}