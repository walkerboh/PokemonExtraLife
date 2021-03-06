﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PokemonExtraLifeApiCore.Models.API
{
    public class PokemonOrder
    {
        public int Id { get; set; }

        [Key]
        [ForeignKey("Pokemon")]
        public int PokemonId { get; set; }

        public int? TrainerId { get; set; }
        public int? GroupId { get; set; }
        public int Sequence { get; set; }
        public bool Activated { get; set; }
        public bool ForceDone { get; set; }
        public bool Done => Pokemon.Damage >= Pokemon.TotalHealth || ForceDone;

        public virtual Pokemon Pokemon { get; set; }
        public virtual Trainer Trainer { get; set; }
        public virtual Group Group { get; set; }
    }
}