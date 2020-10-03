using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Timers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PokemonExtraLifeApiCore.Common;
using PokemonExtraLifeApiCore.EntityFramework;
using PokemonExtraLifeApiCore.Enum;
using PokemonExtraLifeApiCore.Models.API;

namespace PokemonExtraLifeApiCore.ExtraLife
{
    public class EFRunner
    {
        //private const string ApiUrl = @"https://www.extra-life.org/api/participants/318475/donations";
        //private const string ApiUrl = @"http://localhost:57363/api/donor";
        private readonly string ApiUrl;
        private readonly Timer timer;

        private readonly ExtraLifeContext context;

        public EFRunner(DbContextOptions<ExtraLifeContext> options, IOptions<ExtraLifeApiSettings> extraLifeApiSettings)
        {
            context = new ExtraLifeContext(options);
            ApiUrl = extraLifeApiSettings.Value.DonationUrl;

            timer = new Timer();
            timer.Elapsed += GetDonations;
            timer.Interval = 20000;
            timer.Enabled = true;

            GetDonations(null, null);
        }

        private async void GetDonations(object source, ElapsedEventArgs e)
        {
            string json = string.Empty;

            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(ApiUrl);

                    HttpResponseMessage response = await client.GetAsync(ApiUrl);

                    if (response.IsSuccessStatusCode) json = await response.Content.ReadAsStringAsync();
                }
                catch (Exception)
                {
                    // Bury it, don't care for now
                }
            }

            List<EFDonation> donations = JsonConvert.DeserializeObject<List<EFDonation>>(json);

            if (donations == null)
                return;

            DateTime mostRecentDonation = DateTime.MinValue;

            if (context.Donations.Any()) mostRecentDonation = context.Donations.Max(d => d.Time);

            donations = donations.Where(d => d.CreatedDateUtc > mostRecentDonation).ToList();

            DisplayStatus displayStatus = context.GetDisplayStatus();

            IEnumerable<Donation> dbDonations;

            if (displayStatus.TrackDonations)
            {
                Gym? currentGym = context.GetCurrentGym();
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

            context.Donations.AddRange(dbDonations);

            context.SaveChanges();
        }
    }
}