namespace PokemonExtraLifeApiCore.Models.API
{
    public class TwitchChannel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public bool Live { get; set; }

        public string Game { get; set; }
    }
}