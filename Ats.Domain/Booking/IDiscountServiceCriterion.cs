using System.Threading.Tasks;

namespace Ats.Domain.Booking
{
    public interface IDiscountServiceCriterion
    {
        string DiscountOfferName { get; }
        Task<bool> CheckForAsync(BookingAggregate booking);
    }
}
