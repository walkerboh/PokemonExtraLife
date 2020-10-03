using System.Linq;
using PokemonExtraLifeApiCore.Models.API;

namespace PokemonExtraLifeApiCore.EntityFramework
{
    public class DonationProcessor : IDonationProcessor
    {
        private readonly ExtraLifeContext _context;

        public DonationProcessor(ExtraLifeContext context)
        {
            _context = context;
        }

        public IDonationDisplayModel GetNextDonation()
        {
            var nextDonation = _context.Donations.OrderBy(d => d.Time).FirstOrDefault(d => !d.Processed);

            return new DonationDisplayModel
            {
                Donation = nextDonation
            };
        }
    }
}