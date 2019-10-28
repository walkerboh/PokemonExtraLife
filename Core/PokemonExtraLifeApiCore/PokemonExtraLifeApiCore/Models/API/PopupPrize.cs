using Newtonsoft.Json;
using System;

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
        public DateTime? EndTime { get; set; }
        public string WiningDonor { get; set; }

        public bool Active()
        {
            return StartTime.HasValue && !EndTime.HasValue;
        }

        public bool Complete()
        {
            return StartTime.HasValue && EndTime.HasValue;
        }
    }
}