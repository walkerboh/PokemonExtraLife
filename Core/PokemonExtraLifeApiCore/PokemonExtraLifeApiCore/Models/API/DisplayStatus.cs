namespace PokemonExtraLifeApiCore.Models.API
{
    public class DisplayStatus
    {
        public int Id { get; set; }

        public int CurrentHostId { get; set; }

        public string CurrentGame { get; set; }

        public string NextGame { get; set; }

        public string FollowingGame { get; set; }

        public decimal DonationGoal { get; set; }

        public bool TrackDonations { get; set; }

        public decimal HealthMultiplier { get; set; } = 1;
    }
}