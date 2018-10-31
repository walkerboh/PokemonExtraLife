using System;
using Newtonsoft.Json;
using PokemonExtraLifeApi.Models.API;

namespace PokemonExtraLifeApi.ExtraLife
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

        public Donation ToDbDonation(Gym? gym)
        {
            return new Donation
            {
                Amount = Amount,
                Donor = DisplayName,
                Message = Message,
                Time = CreatedDateUtc,
                Gym = gym
            };
        }
    }
}