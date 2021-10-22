using System.Collections.Generic;
using PokemonExtraLifeApiCore.Models.API;

namespace PokemonExtraLifeApiCore.EntityFramework.Initialization
{
    public static class TwitchChannelInitialization
    {
        public static List<TwitchChannel> TwitchChannels = new List<TwitchChannel>
        {
            new TwitchChannel
            {
                Id = 1,
                Name = "Dystortion",
                Url = "https://www.twitch.tv/dystortion"
            },
            new TwitchChannel
            {
                Id = 2,
                Name = "WalkerBoh_",
                Url = "https://www.twitch.tv/walkerboh_"
            }
        };
    }
}