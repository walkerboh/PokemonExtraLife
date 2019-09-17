using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PokemonExtraLifeApiCore.Common;
using PokemonExtraLifeApiCore.EntityFramework;
using PokemonExtraLifeApiCore.Enum;
using PokemonExtraLifeApiCore.Models.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PokemonExtraLifeApiCore.ExtraLife
{
    internal interface IScopedProcessingService
    {
        Task DoWork();
    }

    public class ExtraLifeScopedService : IScopedProcessingService
    {
        private readonly ExtraLifeApiSettings _settings;
        private readonly ExtraLifeContext _context;

        public ExtraLifeScopedService(IOptions<ExtraLifeApiSettings> settings, ExtraLifeContext context)
        {
            _settings = settings.Value;
            _context = context;
        }

        public async Task DoWork()
        {
            string json = string.Empty;

            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(_settings.DonationUrl);

                    HttpResponseMessage response = await client.GetAsync(_settings.DonationUrl);

                    if (response.IsSuccessStatusCode) json = await response.Content.ReadAsStringAsync();
                }
                catch (Exception ex)
                {
                    // Bury it, don't care for now
                }
            }

            List<EFDonation> donations = JsonConvert.DeserializeObject<List<EFDonation>>(json);

            if (donations == null)
                return;

            DateTime mostRecentDonation = DateTime.MinValue;

            if (_context.Donations.Any()) mostRecentDonation = _context.Donations.Max(d => d.Time);

            donations = donations.Where(d => d.CreatedDateUtc > mostRecentDonation).ToList();

            DisplayStatus displayStatus = _context.GetDisplayStatus();

            IEnumerable<Donation> dbDonations;

            if (displayStatus.TrackDonations)
            {
                Gym? currentGym = _context.GetCurrentGym();
                dbDonations = donations.Select(d => d.ToDbDonation(currentGym)).ToList();
            }
            else
            {
                dbDonations = donations.Select(d => d.ToDbDonation(null)).ToList();
                foreach (var d in dbDonations)
                {
                    d.Processed = true;
                }
            }

            _context.Donations.AddRange(dbDonations);

            _context.SaveChanges();
        }
    }
}
