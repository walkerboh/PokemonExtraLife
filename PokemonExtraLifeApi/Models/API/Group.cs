using System.Collections.Generic;
using System.Linq;

namespace PokemonExtraLifeApi.Models.API
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Gym Gym { get; set; }
        public bool Started { get; set; }
        public bool ForceComplete { get; set; }
        public bool PokemonComplete => PokemonOrders.All(po => po.Done);
        public bool Done => ForceComplete || PokemonComplete;
        
        public virtual ICollection<PokemonOrder> PokemonOrders { get; set; }
    }
}