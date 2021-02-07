using Ats.Core.Commands;
using Ats.Core.Domain;
using Ats.Domain.Booking;
using Ats.Domain.FlightInstance;
using System;
using System.Threading.Tasks;

namespace Ats.Application.Booking
{
    public class BookingCommandHandlers :
        ICommandHandler<StartBookingCommand>,
        ICommandHandler<RefreshDiscountOffersCommand>,
        ICommandHandler<CancelBookingCommand>,
        ICommandHandler<ConfirmBookingCommand>
    {
        private readonly BookingStartingService _bookingService;
        private readonly DiscountService _discountService;
        private readonly IRepository<BookingAggregate> _bookingRepository;
        private readonly IRepository<FlightInstanceAggregate> _flightInstanceRepository;

        public BookingCommandHandlers(
            BookingStartingService bookingService,
            DiscountService discountService,
            IRepository<BookingAggregate> bookingRepository,
            IRepository<FlightInstanceAggregate> flightInstanceRepository)
        {
            _bookingService = bookingService ?? throw new ArgumentNullException(nameof(bookingService));
            _discountService = discountService ?? throw new ArgumentNullException(nameof(discountService));
            _bookingRepository = bookingRepository ?? throw new ArgumentNullException(nameof(bookingRepository));
            _flightInstanceRepository = flightInstanceRepository ?? throw new ArgumentNullException(nameof(flightInstanceRepository));
        }

        public async Task HandleAsync(StartBookingCommand command)
        {
            var flightInstance = await _flightInstanceRepository.GetAsync(command.FlightInstanceId);
            var booking = await _bookingRepository.GetAsync(command.BookingId);

            _bookingService.StartBooking(flightInstance, booking, command.BookingId);

            await _flightInstanceRepository.SaveAsync(command.FlightInstanceId, flightInstance, command.FlightInstanceVersion);
            await _bookingRepository.SaveAsync(command.BookingId, booking, 0);
        }

        public async Task HandleAsync(RefreshDiscountOffersCommand command)
        {
            var booking = await _bookingRepository.GetAsync(command.BookingId);

            await _discountService.RefreshDiscountOffersAsync(booking);

            await _bookingRepository.SaveAsync(command.BookingId, booking, command.BookingVersion);
        }

        public async Task HandleAsync(CancelBookingCommand command)
        {
            var booking = await _bookingRepository.GetAsync(command.BookingId);

            booking.Cancel();

            await _bookingRepository.SaveAsync(command.BookingId, booking, command.BookingVersion);
        }

        public async Task HandleAsync(ConfirmBookingCommand command)
        {
            var booking = await _bookingRepository.GetAsync(command.BookingId);

            booking.Confirm();

            await _bookingRepository.SaveAsync(command.BookingId, booking, command.BookingVersion);
        }
    }
}
