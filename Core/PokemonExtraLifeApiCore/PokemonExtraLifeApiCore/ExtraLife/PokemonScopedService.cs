using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PokemonExtraLifeApiCore.Common;
using PokemonExtraLifeApiCore.EntityFramework;
using PokemonExtraLifeApiCore.Models.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Serilog;

namespace PokemonExtraLifeApiCore.ExtraLife
{
    public class PokemonScopedService : IScopedProcessingService
    {
        private readonly ExtraLifeApiSettings _settings;
        private readonly ExtraLifeContext _context;
        private readonly ILogger _logger;

        public PokemonScopedService(IOptions<ExtraLifeApiSettings> settings, ExtraLifeContext context, ILogger logger)
        {
            _settings = settings.Value;
            _context = context;
            _logger = logger;
        }

        public async Task DoWork(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                await Run();

                await Task.Delay(_settings.RequestDelay, stoppingToken);
            }
        }

        public async Task Run()
        {
            var json = string.Empty;

            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(_settings.DonationUrl);

                    var response = await client.GetAsync(_settings.DonationUrl);

                    if (response.IsSuccessStatusCode) json = await response.Content.ReadAsStringAsync();
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Fuck this API");
                }
            }

            var donations = JsonConvert.DeserializeObject<List<EFDonation>>(json);

            if (donations == null)
            {
                return;
            }

            donations.ForEach(d => d.RoundCreatedTime());

            var mostRecentDonation = DateTime.MinValue;

            if (_context.Donations.Any())
            {
                mostRecentDonation = DateTime.SpecifyKind(_context.Donations.Max(d => d.Time), DateTimeKind.Utc);
            }

            donations = donations.Where(d => d.CreatedDateUtc > mostRecentDonation).ToList();

            var displayStatus = _context.GetDisplayStatus();

            IEnumerable<Donation> dbDonations;

            if (displayStatus.TrackDonations)
            {
                var currentGym = _context.GetCurrentGym();
                dbDonations = donations.Select(d => d.ToDbDonation(currentGym)).ToList();
                var prizeId = _context.GetCurrentPrizeId();
                foreach(var d in dbDonations)
                {
                    d.PrizeId = prizeId;
                }
            }
            else
            {
                dbDonations = donations.Select(d => d.ToDbDonation(null)).ToList();
                foreach (var d in dbDonations)
                {
                    d.Processed = true;
                }
            }

            await _context.Donations.AddRangeAsync(dbDonations);

            await _context.SaveChangesAsync();
        }
    }
}
