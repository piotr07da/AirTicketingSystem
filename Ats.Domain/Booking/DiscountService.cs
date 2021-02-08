using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ats.Domain.Booking
{
    public class DiscountService
    {
        private readonly IEnumerable<IDiscountServiceCriterion> _discountCriterions;

        public DiscountService(IEnumerable<IDiscountServiceCriterion> discountCriterions)
        {
            _discountCriterions = discountCriterions ?? throw new ArgumentNullException(nameof(discountCriterions));
        }

        public async Task RefreshDiscountOffersAsync(BookingAggregate booking)
        {
            foreach (var criterion in _discountCriterions)
            {
                if (await criterion.CheckForAsync(booking))
                {
                    if (!booking.DiscountOffers.ContainsKey(criterion.DiscountOfferName))
                    {
                        booking.AddDiscountOffer(new DiscountOffer(criterion.DiscountOfferName, 5.00m));
                    }
                }
                else
                {
                    if (booking.DiscountOffers.ContainsKey(criterion.DiscountOfferName))
                    {
                        booking.RemoveDiscountOffer(criterion.DiscountOfferName);
                    }
                }
            }
        }
    }
}
