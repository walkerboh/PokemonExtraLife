namespace PokemonExtraLifeApi.Models.API
{
    public class DisplayStatus
    {
        public int CurrentHostId { get; set; }

        public string CurrentGame { get; set; }

        public string NextGame { get; set; }

        public string FollowingGame { get; set; }

        public decimal DonationGoal { get; set; }
    }
}