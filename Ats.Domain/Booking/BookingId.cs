using System;

namespace Ats.Domain.Booking
{
    public struct BookingId
    {
        private readonly Guid _id;

        private BookingId(Guid id)
        {
            _id = id;
        }

        public bool IsUndefined => _id.IsUndefined();

        public bool IsDefined => !IsUndefined;

        public static implicit operator Guid(BookingId id) => id._id;
        public static implicit operator BookingId(Guid id) => new BookingId(id);
    }
}
