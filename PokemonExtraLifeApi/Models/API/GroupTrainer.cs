using System.Collections.Generic;

namespace PokemonExtraLifeApi.Models.API
{
    public class GroupTrainer : Trainer
    {
        public int GroupId { get; set; }

        public virtual Group Group { get; set; }
        
        public virtual ICollection<PokemonOrder> PokemonOrders { get; set; }
    }
}