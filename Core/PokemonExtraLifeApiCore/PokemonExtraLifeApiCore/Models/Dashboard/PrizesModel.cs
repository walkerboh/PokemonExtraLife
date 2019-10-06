using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using PokemonExtraLifeApiCore.Models.API;

namespace PokemonExtraLifeApiCore.Models.Dashboard
{
    public class PrizesModel
    {
        public List<PopupPrize> Prizes { get; set; }
        public int? ActivePrizeId { get; set; }
        public int ActivePrizeDonations { get; set; }

        public PopupPrize ActivePrize =>
            ActivePrizeId.HasValue ? Prizes.Single(p => p.Id.Equals(ActivePrizeId.Value)) : null;

        public IEnumerable<SelectListItem> PrizesListItems =>
            Prizes.Where(p => !p.Active() && !p.Complete()).Select(p => new SelectListItem { Text = $"{p.Name}", Value = p.Id.ToString() });

        public IEnumerable<PopupPrize> CompletedPrizes => Prizes.Where(p => p.Complete());
    }
}