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
            List<Group> groups = new List<Group>
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

            List<Giveaway> giveaways = new List<Giveaway>
            {
                new Giveaway
                {
                    Gym = Gym.Rock,
                    PrizeName = "Peepachu Plush - Anubis Studios",
                    Url = "https://i.imgur.com/mQVyS7E.png"
                },
                new Giveaway
                {
                    Gym = Gym.Water,
                    PrizeName = "Misty Art Print - Yueko",
                    Url = "https://i.imgur.com/1viKfEj.png"
                },
                new Giveaway
                {
                    Gym = Gym.Electric,
                    PrizeName = "????",
                    Url = ""
                },
                new Giveaway
                {
                    Gym = Gym.Grass,
                    PrizeName = "Legend of Zelda Art Print + Korok Plush - Pixel Noise Studios + Bubble Rhapsody Design",
                    Url = "https://i.imgur.com/xay4dfS.png"
                },
                new Giveaway
                {
                    Gym = Gym.Poison,
                    PrizeName = "Fat Chocobo Mug + Chocobo Charm",
                    Url = "https://i.imgur.com/Qoo63Dv.png"
                },
                new Giveaway
                {
                    Gym = Gym.Psychic,
                    PrizeName = "???",
                    Url = ""
                },
                new Giveaway
                {
                    Gym = Gym.Fire,
                    PrizeName = "????",
                    Url = ""
                },
                new Giveaway
                {
                    Gym = Gym.Ground,
                    PrizeName = "????",
                    Url = ""
                },
                new Giveaway
                {
                    Gym = Gym.EliteFour,
                    PrizeName = "?????",
                    Url = ""
                },
                new Giveaway
                {
                    Gym = Gym.TeamRocket,
                    PrizeName = "???",
                    Url = ""
                },
                new Giveaway
                {
                    Gym = Gym.TeamRocketRematch,
                    PrizeName = "???",
                    Url = ""
                },
                new Giveaway
                {
                    Gym = Gym.Lavender,
                    PrizeName = "?????",
                    Url = ""
                }
            };

            context.Giveaways.AddRange(giveaways);
            context.SaveChanges();
        }
    }
}