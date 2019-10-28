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
                PrizeName = "Pokemon Prize Pack #1",
                Contributor = "Alyssa, Spicy Doodles, & Dystortion",
                Url = "Url 1",
                Gym = Gym.Flying
            },
            new Giveaway
            {
                Id= 2,
                PrizeName = "Mario Bundle",
                Contributor = "Alyssa & Pixels2dio",
                Url = "Url ",
                Gym = Gym.Bug
            },
            new Giveaway
            {
                Id= 3,
                PrizeName = "SSBU Mural",
                Contributor = "Pixels2dio",
                Url = "Url 1",
                Gym = Gym.Normal
            },
            new Giveaway
            {
                Id= 4,
                PrizeName = "Resident Evil 2 Bundle",
                Contributor = "Animus Rhythm & Crowsmack",
                Url = "Url 1",
                Gym = Gym.Ghost
            },
            new Giveaway
            {
                Id= 5,
                PrizeName = "Pokemon Prize Pack #2",
                Contributor = "Alyssa, Pixels2dio, & Dystortion",
                Url = "Url 1",
                Gym = Gym.Fighting
            },
            new Giveaway
            {
                Id= 6,
                PrizeName = "Fire Emblem Bundle",
                Contributor = "KrazehKai & Naytendo",
                Url = "Url 1",
                Gym = Gym.Steel
            },
            new Giveaway
            {
                Id= 7,
                PrizeName = "Final Fantasy Tea Bundle",
                Contributor = "Ivory Monocle Tea & The Sweetie Bee",
                Url = "Url 1",
                Gym = Gym.Ice
            },
            new Giveaway
            {
                Id= 8,
                PrizeName = "Zelda Prints",
                Contributor = "Dystortion",
                Url = "Url 1",
                Gym = Gym.Dragon
            },
            new Giveaway
            {
                Id= 9,
                PrizeName = "Ghibli Bundle",
                Contributor = "Alyssa, Penelope Love Prints, & AmberCurio",
                Url = "Url 1",
                Gym = Gym.EliteFourWill
            },
            new Giveaway
            {
                Id= 10,
                PrizeName = "Overwatch Prints",
                Contributor = "Art of Chow",
                Url = "Url 1",
                Gym = Gym.EliteFourKoga
            },
            new Giveaway
            {
                Id= 11,
                PrizeName = "Zelda Bundle",
                Contributor = "Vivid Delights, Pixels2dio, & Friendly Fiends Design",
                Url = "Url 1",
                Gym = Gym.EliteFourBruno
            },
            new Giveaway
            {
                Id= 12,
                PrizeName = "Pokemon Prize Pack #3",
                Contributor = "Pixels2dio & Dystortion",
                Url = "Url 1",
                Gym = Gym.EliteFourKaren
            },
            new Giveaway
            {
                Id= 13,
                PrizeName = "MH Print & Dragonite Figure",
                Contributor = "Crowsmack & Dystortion",
                Url = "Url 1",
                Gym = Gym.EliteFourLance
            },
            new Giveaway
            {
                Id= 14,
                PrizeName = "Pokemon Prize Pack #4",
                Contributor = "Alyssa, Pixels2dio, & Dystortion",
                Url = "Url 1",
                Gym = Gym.EliteFourRed
            },
            new Giveaway
            {
                Id= 15,
                PrizeName = "Team Rocket Bundle",
                Contributor = "Dystortion",
                Url = "Url 1",
                Gym = Gym.TeamRocket
            }
        };
    }
}