using System.Collections.Generic;
using PokemonExtraLifeApiCore.Models.API;

namespace PokemonExtraLifeApiCore.EntityFramework.Initialization
{
    public static class HostInitialization
    {
        public static List<Host> Hosts = new List<Host>
        {
            new Host
            {
                Id = 1,
                Name = "Chase",
                Pokemon = "Gengar"
            },
            new Host
            {
                Id = 2,
                Name = "Chrissie",
                Pokemon = "Altaria"
            },
            new Host
            {
                Id = 3,
                Name = "Nick",
                Pokemon = "Dratini"
            },
            new Host
            {
                Id = 4,
                Name = "Joe",
                Pokemon = "Arcanine"
            },
            new Host
            {
                Id = 5,
                Name = "Tim",
                Pokemon = "Mimikyu"
            },
            new Host
            {
                Id = 6,
                Name = "Evan",
                Pokemon = "Torchic"
            },
            new Host
            {
                Id = 7,
                Name = "Brandi",
                Pokemon = "Cubone"
            },
            new Host
            {
                Id = 8,
                Name = "Mikey",
                Pokemon = "Jolteon"
            },
            new Host
            {
                Id = 9,
                Name = "Ethan",
                Pokemon = "Caracosta"
            }
        };
    }
}