using System.Collections.Generic;
using PokemonExtraLifeApi.Models.API;

namespace PokemonExtraLifeApi.EntityFramework
{
    public class ExtraLifeInitializer : System.Data.Entity.DropCreateDatabaseAlways<ExtraLifeContext>
    {
        protected override void Seed(ExtraLifeContext context)
        {
            var pokemon = new List<Pokemon>
            {
                new Pokemon
                {
                    Name = "Bulbasaur",
                    Health = 0
                },
                new Pokemon
                {
                    Name = "Squirtle",
                    Health = 0
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
                    Health = 10
                },
                new Pokemon
                {
                    Name = "B",
                    Health = 10
                },
                new Pokemon
                {
                    Name = "C",
                    Health = 20
                },
                new Pokemon
                {
                    Name = "D",
                    Health = 15
                },
                new Pokemon
                {
                    Name = "E",
                    Health = 15
                },
                new Pokemon
                {
                    Name = "F",
                    Health = 30
                },
                new Pokemon
                {
                    Name = "G",
                    Health = 20
                },
                new Pokemon
                {
                    Name = "H",
                    Health = 20
                },
                new Pokemon
                {
                    Name = "I",
                    Health = 50
                }
            };

            context.Pokemon.AddRange(pokemon);
            context.SaveChanges();

            var trainers = new List<Trainer>
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
                }
            };

            context.Trainers.AddRange(trainers);
            context.SaveChanges();


            var po = new List<PokemonOrder>
            {
                new PokemonOrder
                {
                    PokemonId = 5,
                    TrainerId = 1,
                    Sequence = 1
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
                }
            };

            context.PokemonOrders.AddRange(po);
            context.SaveChanges();

            var hosts = new List<Host>
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
                DonationGoal = 0,
                FollowingGame = string.Empty,
                NextGame = string.Empty
            });
            context.SaveChanges();
        }
    }
}