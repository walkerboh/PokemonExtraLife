using System.Collections.Generic;
using PokemonExtraLifeApiCore.Enum;
using PokemonExtraLifeApiCore.Models.API;

namespace PokemonExtraLifeApiCore.EntityFramework.Initialization
{
    public static class GroupInitialization
    {
        public static List<Group> Groups = new List<Group>
        {
            new Group
            {
                Id = 1,
                Name = "Team Rocket",
                Gym = Gym.TeamRocket
            },
            new Group
            {
                Id = 2,
                Name = "Team Rocket Rematch",
                Gym = Gym.TeamRocketRematch
            },
            new Group
            {
                Id=3,
                Name = "Lavender Town",
                Gym = Gym.Lavender
            }
        };

    }
}