using System.Collections;
using System.Collections.Generic;
using PokemonExtraLifeApi.Models.API;

namespace PokemonExtraLifeApi.Models.Dashboard
{
    public class GroupModel
    {
        public Group ActiveGroup { get; set; }

        public IEnumerable<Group> PotentialGroups { get; set; }
        
        public IEnumerable<Group> PreviouslyActiveGroups { get; set; }
    }
}