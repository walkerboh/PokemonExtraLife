using System.Collections.Generic;
using PokemonExtraLifeApiCore.Models.API;

namespace PokemonExtraLifeApiCore.EntityFramework.Initialization
{
    public static class TargetPrizeInitialization
    {
        public static IEnumerable<TargetPrize> TargetPrizes = new List<TargetPrize>
        {
            new TargetPrize
            {
                Id = 1,
                Name = "test prize",
                Target = 5.67m
            },
            new TargetPrize
            {
                Id = 2,
                Name="No go prize",
                Target = 10000m
            }
        };
    }
}