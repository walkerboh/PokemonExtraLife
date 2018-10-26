using System.Collections.Generic;
using PokemonExtraLifeApi.Models.API;

namespace PokemonExtraLifeApi.Models.Dashboard
{
    public class DonationsModel
    {
        public IEnumerable<Donation> Donations { get; set; }
    }
}