using System;
using System.Collections.Generic;
using System.Linq;
using PokemonExtraLifeApiCore.Enum;

namespace PokemonExtraLifeApiCore.Models.API
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Gym Gym { get; set; }
        public bool Started { get; set; }
        public bool ForceComplete { get; set; }
        public bool PokemonComplete => PokemonOrders.All(po => po.Done);
        public bool Done => ForceComplete || PokemonComplete || DurationMinutes > 0 && StartTime.HasValue && StartTime.Value.AddMinutes(DurationMinutes) < DateTime.Now;
        public int DurationMinutes { get; set; }
        public DateTime? StartTime { get; set; }

        public virtual ICollection<PokemonOrder> PokemonOrders { get; set; }
    }
}