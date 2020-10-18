using PokemonExtraLifeApiCore.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using PokemonExtraLifeApiCore.Common;
using PokemonExtraLifeApiCore.Extensions;
using Serilog;

namespace PokemonExtraLifeApiCore.ExtraLife
{
    public class ExtraLifeScopedService : IScopedProcessingService
    {
        private readonly ExtraLifeApiSettings _settings;
        private readonly ExtraLifeContext _context;
        private readonly ILogger _logger;

        public ExtraLifeScopedService(IOptions<ExtraLifeApiSettings> settings, ExtraLifeContext context, ILogger logger)
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
            List<EFDonation> donations;

            try
            {
                using var client = new HttpClient();
                var response = await client.GetAsync(_settings.DonationUrl);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.Error($"Response from ExtraLife API not successful {response.StatusCode}");
                    return;
                }

                donations = await response.Content.ReadAsAsync<List<EFDonation>>();
            }
            catch (Exception e)
            {
                _logger.Error(e, "Bad ExtraLife API Things");
                return;
            }

            if (donations == null || !donations.Any())
            {
                return;
            }

            donations.ForEach(d => d.RoundCreatedTime());

            var mostRecentDonation = DateTime.MinValue;

            if(_context.Donations.Any())
            {
                mostRecentDonation = DateTime.SpecifyKind(_context.Donations.Max(d => d.Time), DateTimeKind.Utc);
            }

            donations = donations.Where(d => d.CreatedDateUtc > mostRecentDonation).ToList();

            var displayStatus = _context.GetDisplayStatus();

            var dbDonations = donations.Select(d => d.ToDbDonation(null, !displayStatus.TrackDonations)).ToList();

            if(displayStatus.TrackDonations)
            {
                foreach(var donation in dbDonations)
                {
                    var activeTargets = _context.ActiveTargetPrizes;

                    var match = activeTargets.FirstOrDefault(p => p.Target.Equals(donation.Amount));

                    if (match != null && match.Donation == null)
                    {
                        match.Donation = donation;
                    }
                }
            }

            await _context.Donations.AddRangeAsync(dbDonations);
            await _context.SaveChangesAsync();
        }
    }
}