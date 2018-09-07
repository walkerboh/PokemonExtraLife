namespace PokemonExtraLifeApi.Models.API
{
    public class Host
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PokemonId { get; set; }

        public virtual Pokemon Pokemon { get; set; }
    }
}