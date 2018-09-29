namespace PokemonExtraLifeApi.Models.API
{
    public class PokemonOrder
    {
        public int Id { get; set; }
        public int PokemonId { get; set; }
        public int TrainerId { get; set; }
        public int Sequence { get; set; }
        public bool ForceDone { get; set; }
        public bool Done => Pokemon.Damage >= Pokemon.Health || ForceDone;

        public virtual Pokemon Pokemon { get; set; }
        public virtual Trainer Trainer { get; set; }
    }
}