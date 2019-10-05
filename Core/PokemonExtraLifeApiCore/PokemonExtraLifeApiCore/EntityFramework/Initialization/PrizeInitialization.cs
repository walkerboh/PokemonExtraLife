using System.Collections.Generic;
using PokemonExtraLifeApiCore.Models.API;

namespace PokemonExtraLifeApiCore.EntityFramework.Initialization
{
    public static class PrizeInitialization
    {
        public static List<PopupPrize> Prizes = new List<PopupPrize>
        {
            new PopupPrize
            {
                Id = 1,
                Name = "Prize 1",
                Contributor = "Human"
            },
            new PopupPrize
            {
                Id = 2,
                Name = "Prize 2",
                Contributor = "Robot",
            },
            new PopupPrize
            {
                Id = 3,
                Name = "Prize 3",
                Contributor = "Cat"
            }
        };
    }
}