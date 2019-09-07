namespace PokemonExtraLifeApiCore.Models.Dashboard
{
    public class DashboardModel
    {
        public GroupModel GroupModel { get; set; }

        public SummaryModel SummaryModel { get; set; }

        public GamesModel GamesModel { get; set; }

        public DonationsModel DonationsModel { get; set; }

        public PokemonStatusModel PokemonStatusModel { get; set; }
    }
}