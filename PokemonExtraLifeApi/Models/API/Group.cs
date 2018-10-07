using System.Collections.Generic;
using System.Linq;

namespace PokemonExtraLifeApi.Models.API
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Started { get; set; }
        public bool ForceComplete { get; set; }
        public bool TrainersComplete => Trainers.All(t => t.Done);
        public bool Done => ForceComplete || TrainersComplete;
        
        public virtual ICollection<GroupTrainer> Trainers { get; set; }
    }
}