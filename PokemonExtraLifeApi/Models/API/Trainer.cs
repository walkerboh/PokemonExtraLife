using System.Collections.Generic;
using System.Linq;

namespace PokemonExtraLifeApi.Models.API
{
    public class Trainer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public Gym Gym { get; set; }

        public bool Done => PokemonOrders.All(po => po.Done);
        
        public virtual ICollection<PokemonOrder> PokemonOrders { get; set; }
    }
}