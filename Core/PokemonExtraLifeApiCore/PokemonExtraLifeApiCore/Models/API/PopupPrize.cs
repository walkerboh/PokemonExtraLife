using System;
using Newtonsoft.Json;

namespace PokemonExtraLifeApiCore.Models.API
{
    public class PopupPrize
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Contributor { get; set; }
        public string Url { get; set; }
        public DateTime? StartTime { get; set; }
        public int? Duration { get; set; }
        public int? DonationId { get; set; }

        public bool Active()
        {
            if (StartTime.HasValue && Duration.HasValue)
            {
                return StartTime.Value.AddMinutes(Duration.Value) > DateTime.UtcNow;
            }

            return false;
        }
    }
}