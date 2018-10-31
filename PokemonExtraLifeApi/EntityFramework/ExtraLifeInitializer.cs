using System.Collections.Generic;
using System.Data.Entity;
using PokemonExtraLifeApi.EntityFramework.Initialization;
using PokemonExtraLifeApi.Models.API;

namespace PokemonExtraLifeApi.EntityFramework
{
    public class ExtraLifeInitializer : CreateDatabaseIfNotExists<ExtraLifeContext>
    {
        protected override void Seed(ExtraLifeContext context)
        {
            var groups = new List<Group>
            {
                new Group
                {
                    Name = "Team Rocket",
                    Gym = Gym.TeamRocket
                },
                new Group
                {
                    Name = "Team Rocket Rematch",
                    Gym = Gym.TeamRocketRematch
                },
                new Group
                {
                    Name = "Lavender Town",
                    Gym = Gym.Lavender
                }
            };

            context.Groups.AddRange(groups);
            context.SaveChanges();

            context.Pokemon.AddRange(PokemonInitialization.Pokemon);
            context.SaveChanges();

            context.Trainers.AddRange(TrainerInitialization.Trainers);
            context.SaveChanges();

            context.PokemonOrders.AddRange(PokemonOrderInitialization.PokemonOrders);
            context.SaveChanges();

//            List<Pokemon> pokemon = new List<Pokemon>
//            {
//                new Pokemon
//                {
//                    Name = "Bulbasaur",
//                    StartingHealth = 0
//                },
//                new Pokemon
//                {
//                    Name = "Squirtle",
//                    StartingHealth = 0
//                },
//                new Pokemon
//                {
//                    Name = "Charmander"
//                },
//                new Pokemon
//                {
//                    Name = "Oddish"
//                },
//                new Pokemon
//                {
//                    Name = "A",
//                    StartingHealth = 10
//                },
//                new Pokemon
//                {
//                    Name = "B",
//                    StartingHealth = 10
//                },
//                new Pokemon
//                {
//                    Name = "C",
//                    StartingHealth = 20
//                },
//                new Pokemon
//                {
//                    Name = "D",
//                    StartingHealth = 15
//                },
//                new Pokemon
//                {
//                    Name = "E",
//                    StartingHealth = 15
//                },
//                new Pokemon
//                {
//                    Name = "F",
//                    StartingHealth = 30
//                },
//                new Pokemon
//                {
//                    Name = "G",
//                    StartingHealth = 20
//                },
//                new Pokemon
//                {
//                    Name = "H",
//                    StartingHealth = 20
//                },
//                new Pokemon
//                {
//                    Name = "I",
//                    StartingHealth = 50
//                },
//                new Pokemon
//                {
//                    Name = "Meowth",
//                    StartingHealth = 50
//                }
//            };
//
//            context.Pokemon.AddRange(pokemon);
//            context.SaveChanges();
//
//            List<Trainer> trainers = new List<Trainer>
//            {
//                new Trainer
//                {
//                    Name = "T1",
//                    Gym = Gym.Rock
//                },
//                new Trainer
//                {
//                    Name = "T2",
//                    Gym = Gym.Rock
//                },
//                new Trainer
//                {
//                    Name = "T3",
//                    Gym = Gym.Rock
//                },
//                new Trainer
//                {
//                    Name = "Rocket1",
//                    Gym = Gym.TeamRocketAlpha
//                }
//            };
//
//            context.Trainers.AddRange(trainers);
//            context.SaveChanges();
//
//
//            List<PokemonOrder> po = new List<PokemonOrder>
//            {
//                new PokemonOrder
//                {
//                    PokemonId = 5,
//                    TrainerId = 1,
//                    Sequence = 1,
//                    Activated = true
//                },
//                new PokemonOrder
//                {
//                    PokemonId = 6,
//                    TrainerId = 1,
//                    Sequence = 2
//                },
//                new PokemonOrder
//                {
//                    PokemonId = 7,
//                    TrainerId = 1,
//                    Sequence = 3
//                },
//                new PokemonOrder
//                {
//                    PokemonId = 8,
//                    TrainerId = 2,
//                    Sequence = 4
//                },
//                new PokemonOrder
//                {
//                    PokemonId = 9,
//                    TrainerId = 2,
//                    Sequence = 4
//                },
//                new PokemonOrder
//                {
//                    PokemonId = 10,
//                    TrainerId = 2,
//                    Sequence = 5
//                },
//                new PokemonOrder
//                {
//                    PokemonId = 11,
//                    TrainerId = 3,
//                    Sequence = 6
//                },
//                new PokemonOrder
//                {
//                    PokemonId = 12,
//                    TrainerId = 3,
//                    Sequence = 7
//                },
//                new PokemonOrder
//                {
//                    PokemonId = 13,
//                    TrainerId = 3,
//                    Sequence = 8
//                },
//                new PokemonOrder
//                {
//                    PokemonId = 14,
//                    TrainerId = 4,
//                    Sequence = 1,
//                    GroupId = 1
//                }
//            };
//
//            context.PokemonOrders.AddRange(po);
//            context.SaveChanges();

            List<Host> hosts = new List<Host>
            {
                new Host
                {
                    Name = "Chase",
                    PokemonId = 1
                },
                new Host
                {
                    Name = "Chrissy",
                    PokemonId = 2
                },
                new Host
                {
                    Name = "Evan",
                    PokemonId = 3
                },
                new Host
                {
                    Name = "Brandi",
                    PokemonId = 4
                }
            };

            context.Hosts.AddRange(hosts);
            context.SaveChanges();

            context.DisplayStatus.Add(new DisplayStatus
            {
                CurrentHostId = 1,
                CurrentGame = string.Empty,
                DonationGoal = 2000,
                FollowingGame = string.Empty,
                NextGame = string.Empty,
                TrackDonations = true
            });
            context.SaveChanges();
        }
    }
}