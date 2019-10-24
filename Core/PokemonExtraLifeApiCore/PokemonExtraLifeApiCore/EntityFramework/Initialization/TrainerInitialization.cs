using System.Collections.Generic;
using PokemonExtraLifeApiCore.Enum;
using PokemonExtraLifeApiCore.Models.API;

namespace PokemonExtraLifeApiCore.EntityFramework.Initialization
{
    public static class TrainerInitialization
    {
        public static List<Trainer> Trainers = new List<Trainer>
        {
            new Trainer
            {
                Id = 1,
                Name = "Faulkner",
                Gym = Gym.Flying
            },
            new Trainer
            {
                Id = 2,
                Name = "Bugsy",
                Gym = Gym.Bug
            },
            new Trainer
            {
                Id = 3,
                Name = "Whitney",
                Gym = Gym.Normal
            },
            new Trainer
            {
                Id = 4,
                Name = "Morty",
                Gym = Gym.Ghost
            },
            new Trainer
            {
                Id = 5,
                Name = "Chuck",
                Gym = Gym.Fighting
            },
            new Trainer
            {
                Id = 6,
                Name = "Jasmine",
                Gym = Gym.Steel
            },
            new Trainer
            {
                Id = 7,
                Name = "Pryce",
                Gym = Gym.Ice
            },
            new Trainer
            {
                Id = 8,
                Name = "Clair",
                Gym = Gym.Dragon
            },
            new Trainer
            {
                Id = 9,
                Name = "Will",
                Gym = Gym.EliteFourWill
            },
            new Trainer
            {
                Id = 10,
                Name = "Koga",
                Gym = Gym.EliteFourKoga
            },
            new Trainer
            {
                Id = 11,
                Name = "Bruno",
                Gym = Gym.EliteFourBruno
            },
            new Trainer
            {
                Id = 12,
                Name = "Karen",
                Gym = Gym.EliteFourKaren
            },
            new Trainer
            {
                Id = 13,
                Name = "Lance",
                Gym = Gym.EliteFourLance
            },
            new Trainer
            {
                Id = 14,
                Name = "Red",
                Gym = Gym.EliteFourRed
            },
            new Trainer
            {
                Id = 15,
                Name = "Team Rocket",
                Gym = Gym.TeamRocket
            },
        };
    }
}