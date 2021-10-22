using PokemonExtraLifeApiCore.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PokemonExtraLifeApiCore.Common;
using PokemonExtraLifeApiCore.Extensions;
using PokemonExtraLifeApiCore.ExtraLife.Responses;
using Serilog;

namespace PokemonExtraLifeApiCore.ExtraLife
{
    public class ExtraLifeScopedService : IScopedProcessingService
    {
        private readonly ExtraLifeApiSettings _extraLifeSettings;
        private readonly TwitchApiSettings _twitchApiSettings;
        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;

        public ExtraLifeScopedService(IOptions<ExtraLifeApiSettings> settings, IOptions<TwitchApiSettings> twitchSettings, IServiceProvider serviceProvider, ILogger logger)
        {
            _extraLifeSettings = settings.Value;
            _logger = logger;
            _serviceProvider = serviceProvider;
            _twitchApiSettings = twitchSettings.Value;
        }

        public async Task DoWork(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                await Run();

                await Task.Delay(_extraLifeSettings.RequestDelay, stoppingToken);
            }
        }

        public async Task Run()
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ExtraLifeContext>();

            await Donations(context);
            await Twitch(context);
        }

        private async Task Donations(ExtraLifeContext context)
        {
            List<EFDonation> donations;

            try
            {
                using var client = new HttpClient();
                var response = await client.GetAsync(_extraLifeSettings.DonationUrl);

                if (!response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    _logger.Error($"Response from ExtraLife API not successful {response.StatusCode}");
                    _logger.Error(content);
                    return;
                }

                donations = await response.Content.ReadAsAsync<List<EFDonation>>();
                _logger.Information($"{donations.Count} donations found");
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

            if (context.Donations.Any())
            {
                mostRecentDonation = DateTime.SpecifyKind(context.Donations.Max(d => d.Time), DateTimeKind.Utc);
            }

            donations = donations.Where(d => d.CreatedDateUtc > mostRecentDonation).ToList();

            var displayStatus = context.GetDisplayStatus();

            var dbDonations = donations.Select(d => d.ToDbDonation(null, !displayStatus.TrackDonations)).ToList();

            if (displayStatus.TrackDonations)
            {
                foreach (var donation in dbDonations.OrderBy(d => d.Time))
                {
                    donation.Block = displayStatus.DonationBlock;
                }
            }

            await context.Donations.AddRangeAsync(dbDonations);
            await context.SaveChangesAsync();
        }

        public async Task Twitch(ExtraLifeContext context)
        {
            try
            {
                using var client = new HttpClient();
                var streams = context.TwitchChannels.ToList();

                foreach (var stream in streams)
                {
                    var request = new HttpRequestMessage
                    {
                        RequestUri = new Uri($@"https://api.twitch.tv/helix/streams?user_login={stream.Name}"),
                        Method = HttpMethod.Get,
                        Headers =
                        {
                            Authorization = new AuthenticationHeaderValue("Bearer", _twitchApiSettings.AccessToken)
                        }
                    };
                    request.Headers.Add("client-id", _twitchApiSettings.ClientId);

                    var response = await client.SendAsync(request);
                    var streamsData = await response.Content.ReadAsAsync<StreamsResponse>();

                    var streamData = streamsData.data.FirstOrDefault(d =>
                        d.user_name.Equals(stream.Name, StringComparison.InvariantCultureIgnoreCase));

                    if(streamData == null)
                    {
                        stream.Live = false;
                        continue;
                    }

                    stream.Live = true;
                    var gameId = streamData.game_id;

                    request = new HttpRequestMessage
                    {
                        RequestUri = new Uri($@"https://api.twitch.tv/helix/games?id={gameId}"),
                        Method = HttpMethod.Get,
                        Headers =
                        {
                            Authorization = new AuthenticationHeaderValue("Bearer", _twitchApiSettings.AccessToken)
                        }
                    };
                    request.Headers.Add("client-id", _twitchApiSettings.ClientId);

                    response = await client.SendAsync(request);
                    var gameData = await response.Content.ReadAsAsync<GameResponse>();

                    stream.Game = gameData.data.FirstOrDefault()?.name;
                }

                await context.SaveChangesAsync();
            }
            catch(Exception e)
            {
                _logger.Error(e, "Bad Twitch API things");
            }
        }
    }
}