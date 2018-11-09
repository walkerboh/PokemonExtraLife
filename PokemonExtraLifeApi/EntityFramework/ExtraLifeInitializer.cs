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
                    Gym = Gym.TeamRocket,
                    DurationMinutes = 60
                },
                new Group
                {
                    Name = "Team Rocket Rematch",
                    Gym = Gym.TeamRocketRematch,
                    DurationMinutes = 60
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
                TrackDonations = false,
                HealthMultiplier = 1
            });
            context.SaveChanges();

            List<Giveaway> giveaways = new List<Giveaway>
            {
                new Giveaway
                {
                    Gym = Gym.Rock,
                    PrizeName = "Peepachu Plush",
                    Contributor = "Anubis Studios",
                    Url = "BrockPrize"
                },
                new Giveaway
                {
                    Gym = Gym.Water,
                    PrizeName = "Misty Art Print",
                    Contributor = "Yueko",
                    Url = "MistyPrize"
                },
                new Giveaway
                {
                    Gym = Gym.Electric,
                    PrizeName = "D.VA Lootbox + D.VA Pokewatch Charm",
                    Contributor = "Aesthetic Cosplay + Orotea",
                    Url = "SurgePrize"
                },
                new Giveaway
                {
                    Gym = Gym.Grass,
                    PrizeName = "Legend of Zelda Art Print + Korok Plush",
                    Contributor = "Pixel Noise Studios + Bubble Rhapsody Design",
                    Url = "ErikaPrize"
                },
                new Giveaway
                {
                    Gym = Gym.Poison,
                    PrizeName = "Fat Chocobo Mug + Chocobo Charm",
                    Contributor = "Animus Rhythm + Hideaway Melon",
                    Url = "KogaPrize"
                },
                new Giveaway
                {
                    Gym = Gym.Psychic,
                    PrizeName = "Dark Souls Art Print",
                    Contributor = "Crowsmack",
                    Url = "SabrinaPrize"
                },
                new Giveaway
                {
                    Gym = Gym.Fire,
                    PrizeName = "Vulpix Plush",
                    Contributor = "Toreba",
                    Url = "BlainePrize"
                },
                new Giveaway
                {
                    Gym = Gym.Ground,
                    PrizeName = "Boss Pikachu + Lanyard",
                    Contributor = "Pokemon Center Official",
                    Url = "GiovanniPrize"
                },
                new Giveaway
                {
                    Gym = Gym.EliteFour,
                    PrizeName = "Mewtwo Figure",
                    Contributor = "Pokemon Center Official",
                    Url = "LeaguePrize"
                },
                new Giveaway
                {
                    Gym = Gym.TeamRocket,
                    PrizeName = "$20 Gamestop Giftcard",
                    Contributor = "Gamestop",
                    Url = "Rocket1Prize"
                },
                new Giveaway
                {
                    Gym = Gym.TeamRocketRematch,
                    PrizeName = "Napstablook Plush + Annoying Dog Pig",
                    Contributor = "Mad Hatter Plushies + Fifthshroom",
                    Url = "Rocket2Prize"
                },
                new Giveaway
                {
                    Gym = Gym.Lavender,
                    PrizeName = "Mimikyu Clock + K'nex Set",
                    Contributor = "Toreba + K'nex",
                    Url = "LavenderPrize"
                }
            };

            context.Giveaways.AddRange(giveaways);
            context.SaveChanges();
        }
    }
}