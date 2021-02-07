using System;

namespace Ats.Domain.FlightInstance
{
    public struct CustomerId
    {
        private readonly Guid _id;

        private CustomerId(Guid id)
        {
            _id = id;
        }

        public bool IsUndefined => _id.IsUndefined();

        public bool IsDefined => !IsUndefined;

        public override string ToString() => _id.ToString();

        public static implicit operator Guid(CustomerId id) => id._id;
        public static implicit operator CustomerId(Guid id) => new CustomerId(id);
    }
}
