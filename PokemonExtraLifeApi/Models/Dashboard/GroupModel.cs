using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PokemonExtraLifeApi.Models.API;

namespace PokemonExtraLifeApi.Models.Dashboard
{
    public class GroupModel
    {
        public Group ActiveGroup { get; set; }

        public IEnumerable<Group> PotentialGroups { get; set; }

        public IEnumerable<SelectListItem> PotentialGroupsList
        {
            get { return PotentialGroups.Select(g => new SelectListItem {Text = g.Name, Value = g.Id.ToString()}); }
        }

        public IEnumerable<Group> PreviouslyActiveGroups { get; set; }

        public int? SelectedGroup { get; set; }
    }
}