using System.Collections.Generic;
using PokemonExtraLifeApiCore.Models.API;

namespace PokemonExtraLifeApiCore.Models.Dashboard
{
    public class DonationsModel
    {
        public IEnumerable<Donation> Donations { get; set; }
    }
}