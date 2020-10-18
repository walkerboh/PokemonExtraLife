using System;
using Newtonsoft.Json;
using PokemonExtraLifeApiCore.Enum;
using PokemonExtraLifeApiCore.Models.API;

namespace PokemonExtraLifeApiCore.ExtraLife
{
    public class EFDonation
    {
        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("avatarImageURL")]
        public string AvatarImageUrl { get; set; }

        [JsonProperty("createdDateUTC")]
        public DateTime CreatedDateUtc { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("donorID")]
        public string DonorId { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        public Donation ToDbDonation(Gym? gym, bool processed = false)
        {
            return new Donation
            {
                Amount = Amount,
                Donor = DisplayName,
                Message = Message,
                Time = CreatedDateUtc,
                Gym = gym,
                Processed = processed
            };
        }

        public void RoundCreatedTime()
        {
            var ticks = CreatedDateUtc.Ticks;
            var roundedTickets = ticks - (ticks % 10);
            CreatedDateUtc = new DateTime(roundedTickets, DateTimeKind.Utc);
        }
    }
}