using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Timers;
using System.Web;
using Newtonsoft.Json;
using PokemonExtraLifeApi.EntityFramework;

namespace PokemonExtraLifeApi.ExtraLife
{
    public class EFRunner
    {
        Timer timer;

        //private const string ApiUrl = @"http://wwww.extra-life.org/api/participants/302374/donations";
        private const string ApiUrl = @"http://localhost:57363/api/donor";

        public EFRunner()
        {
            timer = new Timer();
            timer.Elapsed += GetDonations;
            timer.Interval = 60000;
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

                    var response = await client.GetAsync(ApiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        json = await response.Content.ReadAsStringAsync();
                    }
                }
                catch (Exception ex)
                {
                    // Bury it, don't care for now
                }
            }

            List<EFDonation> donations = JsonConvert.DeserializeObject<List<EFDonation>>(json);

            using (var context = new ExtraLifeContext())
            {
                DateTime mostRecentDonation = DateTime.MinValue;

                if (context.Donations.Any())
                {
                    mostRecentDonation = context.Donations.Max(d => d.Time);
                }

                donations = donations.Where(d => d.CreatedDateUtc > mostRecentDonation).ToList();

                var currentGym = context.GetCurrentGym();
                
                context.Donations.AddRange(donations.Select(d => d.ToDbDonation(currentGym)));

                context.SaveChanges();
            }
        }
    }
}