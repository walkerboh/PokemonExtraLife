using System.Collections.Generic;
using System.Data.Entity;
using PokemonExtraLifeApi.Models.API;

namespace PokemonExtraLifeApi.EntityFramework
{
    public class ExtraLifeInitializer : CreateDatabaseIfNotExists<ExtraLifeContext>
    {
        protected override void Seed(ExtraLifeContext context)
        {
            context.Groups.Add(new Group
            {
                Name = "Rocket 1",
                Gym = Gym.TeamRocketAlpha
            });
            context.SaveChanges();

            List<Pokemon> pokemon = new List<Pokemon>
            {
                new Pokemon
                {
                    Name = "Bulbasaur",
                    TotalHealth = 0
                },
                new Pokemon
                {
                    Name = "Squirtle",
                    TotalHealth = 0
                },
                new Pokemon
                {
                    Name = "Charmander"
                },
                new Pokemon
                {
                    Name = "Oddish"
                },
                new Pokemon
                {
                    Name = "A",
                    TotalHealth = 10
                },
                new Pokemon
                {
                    Name = "B",
                    TotalHealth = 10
                },
                new Pokemon
                {
                    Name = "C",
                    TotalHealth = 20
                },
                new Pokemon
                {
                    Name = "D",
                    TotalHealth = 15
                },
                new Pokemon
                {
                    Name = "E",
                    TotalHealth = 15
                },
                new Pokemon
                {
                    Name = "F",
                    TotalHealth = 30
                },
                new Pokemon
                {
                    Name = "G",
                    TotalHealth = 20
                },
                new Pokemon
                {
                    Name = "H",
                    TotalHealth = 20
                },
                new Pokemon
                {
                    Name = "I",
                    TotalHealth = 50
                },
                new Pokemon
                {
                    Name = "Meowth",
                    TotalHealth = 50
                }
            };

            context.Pokemon.AddRange(pokemon);
            context.SaveChanges();

            List<Trainer> trainers = new List<Trainer>
            {
                new Trainer
                {
                    Name = "T1",
                    Gym = Gym.Rock
                },
                new Trainer
                {
                    Name = "T2",
                    Gym = Gym.Rock
                },
                new Trainer
                {
                    Name = "T3",
                    Gym = Gym.Rock
                },
                new Trainer
                {
                    Name = "Rocket1",
                    Gym = Gym.TeamRocketAlpha
                }
            };

            context.Trainers.AddRange(trainers);
            context.SaveChanges();


            List<PokemonOrder> po = new List<PokemonOrder>
            {
                new PokemonOrder
                {
                    PokemonId = 5,
                    TrainerId = 1,
                    Sequence = 1,
                    Activated = true
                },
                new PokemonOrder
                {
                    PokemonId = 6,
                    TrainerId = 1,
                    Sequence = 2
                },
                new PokemonOrder
                {
                    PokemonId = 7,
                    TrainerId = 1,
                    Sequence = 3
                },
                new PokemonOrder
                {
                    PokemonId = 8,
                    TrainerId = 2,
                    Sequence = 4
                },
                new PokemonOrder
                {
                    PokemonId = 9,
                    TrainerId = 2,
                    Sequence = 4
                },
                new PokemonOrder
                {
                    PokemonId = 10,
                    TrainerId = 2,
                    Sequence = 5
                },
                new PokemonOrder
                {
                    PokemonId = 11,
                    TrainerId = 3,
                    Sequence = 6
                },
                new PokemonOrder
                {
                    PokemonId = 12,
                    TrainerId = 3,
                    Sequence = 7
                },
                new PokemonOrder
                {
                    PokemonId = 13,
                    TrainerId = 3,
                    Sequence = 8
                },
                new PokemonOrder
                {
                    PokemonId = 14,
                    TrainerId = 4,
                    Sequence = 1,
                    GroupId = 1
                }
            };

            context.PokemonOrders.AddRange(po);
            context.SaveChanges();

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