using System;

namespace Ats.Domain
{
    public class BookingId
    {
        private Guid _guid;

        private BookingId(Guid guid)
        {
            if (guid == Guid.Empty) throw new ArgumentException(nameof(guid));

            _guid = guid;
        }

        public override string ToString()
        {
            return _guid.ToString();
        }

        public Guid ToGuid()
        {
            return _guid;
        }

        public static BookingId FromString(string value)
        {
            return FromGuid(new Guid(value));
        }

        public static BookingId FromGuid(Guid guid)
        {
            return new BookingId(guid);
        }
    }
}
