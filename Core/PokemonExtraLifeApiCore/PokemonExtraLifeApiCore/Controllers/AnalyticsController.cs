using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PokemonExtraLifeApiCore.EntityFramework;
using PokemonExtraLifeApiCore.Models.API;

namespace PokemonExtraLifeApiCore.Controllers
{
    public class AnalyticsController : Controller
    {
        private readonly ExtraLifeContext _context;

        public AnalyticsController(ExtraLifeContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult HourlyChart()
        {
            var startTime = new DateTime(2019, 11, 2, 11, 00, 00);
            var endTime = new DateTime(2019, 11, 3, 11, 00, 00);

            var donations = _context.Donations;

            var preDonations = donations.Where(d => d.Time < startTime).ToList();
            var postDonations = donations.Where(d => d.Time > endTime).ToList();
            var duringDonations = donations.Where(d => d.Time > startTime && d.Time < endTime).ToList();

            var csv = new StringBuilder();
            csv.AppendLine("Hour, count, total");
            csv.AppendLine($"Pre Show, {preDonations.Count}, {preDonations.Sum(d => d.Amount)}");
            foreach (var donation in duringDonations.GroupBy(d => d.Time.Hour))
            {
                csv.AppendLine($"{donation.Key}, {donation.Count()}, {donation.Sum(d=>d.Amount)}");
            }

            if (postDonations.Any())
            {
                csv.AppendLine($"Post Show, {postDonations.Count}, {postDonations.Sum(d => d.Amount)}");
            }


            return Content(csv.ToString());
        }

        public ActionResult GymChart()
        {
            var donations = _context.Donations.Where(d => d.Gym.HasValue).GroupBy(d => d.Gym);

            var csv = new StringBuilder();
            csv.AppendLine("Gym, count, total");

            foreach(var donation in donations)
            {
                csv.AppendLine($"{donation.Key.ToString()}, {donation.Count()}, {donation.Sum(d => d.Amount)}");
            }

            return Content(csv.ToString());
        }
    }
}