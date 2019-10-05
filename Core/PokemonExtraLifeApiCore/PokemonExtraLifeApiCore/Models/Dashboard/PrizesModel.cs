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

        public PopupPrize ActivePrize =>
            ActivePrizeId.HasValue ? Prizes.Single(p => p.Id.Equals(ActivePrizeId.Value)) : null;

        public IEnumerable<SelectListItem> PrizesListItems =>
            Prizes.Where(p => !p.StartTime.HasValue).Select(p => new SelectListItem { Text = $"{p.Name} ({GetPrizeStatus(p)})", Value = p.Id.ToString() });

        private static string GetPrizeStatus(PopupPrize prize)
        {
            if (prize.Active())
            {
                return "Active";
            }
            else if (prize.Duration.HasValue && !prize.Active())
            {
                return "Complete";
            }
            else
            {
                return "Pending";
            }
        }
    }
}