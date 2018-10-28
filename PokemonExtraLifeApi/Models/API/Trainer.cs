using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace PokemonExtraLifeApi.Models.API
{
    public class Trainer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public Gym Gym { get; set; }
        public bool Leader { get; set; }

        public int PokemonLeft => PokemonOrders.Count(po => !po.Done);
        public bool Done => PokemonOrders.All(po => po.Done);

        [JsonIgnore]
        public virtual ICollection<PokemonOrder> PokemonOrders { get; set; }
    }
}