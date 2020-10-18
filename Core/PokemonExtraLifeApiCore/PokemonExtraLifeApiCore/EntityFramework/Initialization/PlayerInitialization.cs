using System.Collections.Generic;
using PokemonExtraLifeApiCore.Models.API;

namespace PokemonExtraLifeApiCore.EntityFramework.Initialization
{
    public static class PlayerInitialization
    {
        public static List<Player> Players = new List<Player>
        {
            new Player {Id = 1},
            new Player {Id = 2},
            new Player {Id = 3},
            new Player {Id = 4},
            new Player {Id = 5},
            new Player {Id = 6},
            new Player {Id = 7},
            new Player {Id = 8},
            new Player {Id = 9},
            new Player {Id = 10}
        };
    }
}