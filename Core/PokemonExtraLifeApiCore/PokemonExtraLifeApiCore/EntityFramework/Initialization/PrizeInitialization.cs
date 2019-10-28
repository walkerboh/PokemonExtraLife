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
                Name = "DedenneCordKeeper",
                Contributor = "Dystortion"
            },
            new PopupPrize
            {
                Id = 2,
                Name = "Diglet",
                Contributor = "Dystortion",
            },
            new PopupPrize
            {
                Id = 3,
                Name = "Ditto",
                Contributor = "Alyssa"
            },
            new PopupPrize
            {
                Id= 4,
                Name = "StarmanStoutGlossPaper",
                Contributor = "Pixels2dio"
            },
            new PopupPrize
            {
                Id= 5,
                Name = "MythraPyra",
                Contributor = "Pixels2dio"
            },
            new PopupPrize
            {
                Id= 6,
                Name = "SSBUMural",
                Contributor = "Pixels2dio"
            },
            new PopupPrize
            {
                Id= 7,
                Name = "WindwakerCanvas",
                Contributor = "Pixels2dio"
            },
            new PopupPrize
            {
                Id= 8,
                Name = "LinkClimbing",
                Contributor = "Pixels2dio"
            },
            new PopupPrize
            {
                Id= 9,
                Name = "LinkMotorcycle",
                Contributor = "Pixels2dio"
            },
        };
    }
}