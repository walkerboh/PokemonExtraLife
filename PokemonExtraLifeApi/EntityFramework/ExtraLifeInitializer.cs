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

            List<Host> hosts = new List<Host>
            {
                new Host
                {
                    Name = "Chase",
                    Pokemon = "Gengar"
                },
                new Host
                {
                    Name = "Chrissie",
                    Pokemon = "Altaria"
                },
                new Host
                {
                    Name = "Nick",
                    Pokemon = "Dratini"
                },
                new Host
                {
                    Name = "Joe",
                    Pokemon = "Arcanine"
                },
                new Host
                {
                    Name = "Tim",
                    Pokemon = "Mimikyu"
                },
                new Host
                {
                    Name = "Evan",
                    Pokemon = "Torchic"
                },
                new Host
                {
                    Name = "Brandi",
                    Pokemon = "Cubone"
                },
                new Host
                {
                    Name = "Mikey",
                    Pokemon = "Jolteon"
                },
                new Host
                {
                    Name = "Ethan",
                    Pokemon = "Caracosta"
                },
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