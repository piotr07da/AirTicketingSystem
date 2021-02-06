using System;

namespace Ats.Domain.FlightInstance
{
    public struct FlightInstanceId
    {
        private readonly Guid _id;

        private FlightInstanceId(Guid id)
        {
            _id = id;
        }

        public bool IsUndefined => _id.IsUndefined();

        public bool IsDefined => !IsUndefined;

        public static implicit operator Guid(FlightInstanceId id) => id._id;
        public static implicit operator FlightInstanceId(Guid id) => new FlightInstanceId(id);
    }
}
