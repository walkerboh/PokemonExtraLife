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
                PrizeName = "Pokemon Garden Party",
                Contributor = "Alyssa, Spicy Doodles, & Dystortion",
                Url = "Url 1",
                Gym = Gym.Flying
            },
            new Giveaway
            {
                Id= 2,
                PrizeName = "Mushroom Kingdom Essentials",
                Contributor = "Alyssa & Pixels2dio",
                Url = "Url ",
                Gym = Gym.Bug
            },
            new Giveaway
            {
                Id= 3,
                PrizeName = "Everyone Is Here!",
                Contributor = "Pixels2dio",
                Url = "Url 1",
                Gym = Gym.Normal
            },
            new Giveaway
            {
                Id= 4,
                PrizeName = "X Is Gonna Give It To Ya",
                Contributor = "Animus Rhythm & Crowsmack",
                Url = "Url 1",
                Gym = Gym.Ghost
            },
            new Giveaway
            {
                Id= 5,
                PrizeName = "Let's Go Pikachu",
                Contributor = "Alyssa, Pixels2dio, & Dystortion",
                Url = "Url 1",
                Gym = Gym.Fighting
            },
            new Giveaway
            {
                Id= 6,
                PrizeName = "Choose Your House",
                Contributor = "KrazehKai & Naytendo",
                Url = "Url 1",
                Gym = Gym.Steel
            },
            new Giveaway
            {
                Id= 7,
                PrizeName = "Final Fantas-tea",
                Contributor = "Ivory Monocle Tea & The Sweetie Bee",
                Url = "Url 1",
                Gym = Gym.Ice
            },
            new Giveaway
            {
                Id= 8,
                PrizeName = "Courage & Wisdom",
                Contributor = "Dystortion",
                Url = "Url 1",
                Gym = Gym.Dragon
            },
            new Giveaway
            {
                Id= 9,
                PrizeName = "Your New Neighbor",
                Contributor = "Alyssa, Penelope Love Prints, & AmberCurio",
                Url = "Url 1",
                Gym = Gym.EliteFourWill
            },
            new Giveaway
            {
                Id= 10,
                PrizeName = "The World Needs Heroes",
                Contributor = "Art of Chow",
                Url = "Url 1",
                Gym = Gym.EliteFourKoga
            },
            new Giveaway
            {
                Id= 11,
                PrizeName = "Light of the Hero",
                Contributor = "Vivid Delights, Pixels2dio, & Friendly Fiends Design",
                Url = "Url 1",
                Gym = Gym.EliteFourBruno
            },
            new Giveaway
            {
                Id= 12,
                PrizeName = "Cuteness Overload",
                Contributor = "Pixels2dio & Dystortion",
                Url = "Url 1",
                Gym = Gym.EliteFourKaren
            },
            new Giveaway
            {
                Id= 13,
                PrizeName = "Master of Dragons",
                Contributor = "Crowsmack & Dystortion",
                Url = "Url 1",
                Gym = Gym.EliteFourLance
            },
            new Giveaway
            {
                Id= 14,
                PrizeName = "The Very Best",
                Contributor = "Alyssa, Pixels2dio, & Dystortion",
                Url = "Url 1",
                Gym = Gym.EliteFourRed
            },
            new Giveaway
            {
                Id= 15,
                PrizeName = "Blasting Off Again",
                Contributor = "Dystortion",
                Url = "Url 1",
                Gym = Gym.TeamRocket
            }
        };
    }
}