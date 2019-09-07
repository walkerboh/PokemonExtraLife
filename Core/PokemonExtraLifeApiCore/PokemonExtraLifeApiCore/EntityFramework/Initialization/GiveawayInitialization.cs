using System.Collections.Generic;
using PokemonExtraLifeApiCore.Enum;
using PokemonExtraLifeApiCore.Models.API;

namespace PokemonExtraLifeApiCore.EntityFramework.Initialization
{
    public static class GiveawayInitialization
    {
        public static List<Giveaway> Giveaways = new List<Giveaway>()
        { 
            new Giveaway
            {
                Id= 1,
                PrizeName = "Giveaway 1",
                Contributor = "Contributor 1",
                Url = "Url 1",
                Gym = Gym.Rock
            },
            new Giveaway
            {
                Id= 2,
                PrizeName = "Giveaway 2",
                Contributor = "Contributor 2",
                Url = "Url 2",
                Gym = Gym.Water
            },
            new Giveaway
            {
                Id= 3,
                PrizeName = "Giveaway 1",
                Contributor = "Contributor 1",
                Url = "Url 1",
                Gym = Gym.Electric
            },
            new Giveaway
            {
                Id= 4,
                PrizeName = "Giveaway 1",
                Contributor = "Contributor 1",
                Url = "Url 1",
                Gym = Gym.Grass
            },
            new Giveaway
            {
                Id= 5,
                PrizeName = "Giveaway 1",
                Contributor = "Contributor 1",
                Url = "Url 1",
                Gym = Gym.Poison
            },
            new Giveaway
            {
                Id= 6,
                PrizeName = "Giveaway 1",
                Contributor = "Contributor 1",
                Url = "Url 1",
                Gym = Gym.Psychic
            },
            new Giveaway
            {
                Id= 7,
                PrizeName = "Giveaway 1",
                Contributor = "Contributor 1",
                Url = "Url 1",
                Gym = Gym.Fire
            },
            new Giveaway
            {
                Id= 8,
                PrizeName = "Giveaway 1",
                Contributor = "Contributor 1",
                Url = "Url 1",
                Gym = Gym.Ground
            },
            new Giveaway
            {
                Id= 9,
                PrizeName = "Giveaway 1",
                Contributor = "Contributor 1",
                Url = "Url 1",
                Gym = Gym.EliteFour
            },
            new Giveaway
            {
                Id= 10,
                PrizeName = "Giveaway 1",
                Contributor = "Contributor 1",
                Url = "Url 1",
                Gym = Gym.TeamRocket
            },
            new Giveaway
            {
                Id= 11,
                PrizeName = "Giveaway 1",
                Contributor = "Contributor 1",
                Url = "Url 1",
                Gym = Gym.TeamRocketRematch
            },
            new Giveaway
            {
                Id= 12,
                PrizeName = "Giveaway 1",
                Contributor = "Contributor 1",
                Url = "Url 1",
                Gym = Gym.Lavender
            },
        };
    }
}