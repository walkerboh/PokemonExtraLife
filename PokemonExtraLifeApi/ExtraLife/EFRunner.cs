using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Timers;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using PokemonExtraLifeApi.EntityFramework;
using PokemonExtraLifeApi.Models.API;

namespace PokemonExtraLifeApi.ExtraLife
{
    public class EFRunner
    {
        //private const string ApiUrl = @"https://www.extra-life.org/api/participants/318475/donations";
        //private const string ApiUrl = @"http://localhost:57363/api/donor";
        private static readonly string ApiUrl = ConfigurationManager.AppSettings["DonationUrl"];
        private readonly Timer timer;

        public EFRunner()
        {
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
                catch (Exception ex)
                {
                    // Bury it, don't care for now
                }
            }

            List<EFDonation> donations = JsonConvert.DeserializeObject<List<EFDonation>>(json);

            if (donations == null)
                return;

            using (var context = new ExtraLifeContext())
            {
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
                    dbDonations.ForEach(d => d.Processed = true);
                }

                context.Donations.AddRange(dbDonations);

                context.SaveChanges();
            }
        }
    }
}