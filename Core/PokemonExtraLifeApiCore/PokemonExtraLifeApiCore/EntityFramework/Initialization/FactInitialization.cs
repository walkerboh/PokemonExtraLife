using System.Collections.Generic;
using PokemonExtraLifeApiCore.Models.API;

namespace PokemonExtraLifeApiCore.EntityFramework.Initialization
{
    public static class FactInitialization
    {
        public static List<Fact> Facts = new List<Fact>
        {
            new Fact
            {
                Id = 1,
                Text = "This is a sample fact."
            },
            new Fact
            {
                Id = 2,
                Text = "There are many facts in the world, but this one is mine."
            },
            new Fact
            {
                Id = 3,
                Text = "You are awesome! Thanks for being here!"
            }
        };
    }
}