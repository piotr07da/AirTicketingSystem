using Ats.Core.Commands;
using Ats.Core.Domain;
using Ats.Domain;
using System;
using System.Threading.Tasks;

namespace Ats.Application
{
    public class BookingCommandHandlers :
        ICommandHandler<StartBookingCommand>,
        ICommandHandler<CancelBookingCommand>,
        ICommandHandler<ConfirmBookingCommand>
    {
        private readonly BookingService _bookingService;
        private readonly IRepository<BookingAggregate> _bookingRepository;
        private readonly IRepository<FlightInstanceAggregate> _flightInstanceRepository;

        public BookingCommandHandlers(
            BookingService bookingService,
            IRepository<BookingAggregate> bookingRepository,
            IRepository<FlightInstanceAggregate> flightInstanceRepository)
        {
            _bookingService = bookingService ?? throw new ArgumentNullException(nameof(bookingService));
            _bookingRepository = bookingRepository ?? throw new ArgumentNullException(nameof(bookingRepository));
            _flightInstanceRepository = flightInstanceRepository ?? throw new ArgumentNullException(nameof(flightInstanceRepository));
        }

        public async Task HandleAsync(StartBookingCommand command)
        {
            var flightInstance = await _flightInstanceRepository.GetAsync(command.FlightInstanceId);
            var booking = await _bookingRepository.GetAsync(command.BookingId);

            _bookingService.StartBooking(flightInstance, booking, command.BookingId);

            await _flightInstanceRepository.SaveAsync(flightInstance.Id, flightInstance, command.FlightInstanceVersion);
            await _bookingRepository.SaveAsync(booking.Id, booking, 0);
        }

        public async Task HandleAsync(CancelBookingCommand command)
        {
            var booking = await _bookingRepository.GetAsync(command.BookingId);

            booking.Cancel();

            await _bookingRepository.SaveAsync(booking.Id, booking, command.BookingVersion);
        }

        public async Task HandleAsync(ConfirmBookingCommand command)
        {
            var booking = await _bookingRepository.GetAsync(command.BookingId);

            booking.Confirm();

            await _bookingRepository.SaveAsync(booking.Id, booking, command.BookingVersion);
        }
    }
}
