using System;

namespace Ats.Domain.Flight
{
    public struct FlightUid
    {
        private readonly Guid _id;

        private FlightUid(Guid id)
        {
            _id = id;
        }

        public bool IsUndefined => _id.IsUndefined();

        public bool IsDefined => !IsUndefined;

        public override string ToString() => _id.ToString();

        public static implicit operator Guid(FlightUid id) => id._id;
        public static implicit operator FlightUid(Guid id) => new FlightUid(id);
    }
}
