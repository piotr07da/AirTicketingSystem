using System;

namespace Ats.Domain.Airports
{
    public struct AirportsId
    {
        private readonly Guid _id;

        private AirportsId(Guid id)
        {
            _id = id;
        }

        public bool IsUndefined => _id.IsUndefined();

        public bool IsDefined => !IsUndefined;

        public override string ToString() => _id.ToString();

        public static implicit operator Guid(AirportsId id) => id._id;
        public static implicit operator AirportsId(Guid id) => new AirportsId(id);
    }
}
