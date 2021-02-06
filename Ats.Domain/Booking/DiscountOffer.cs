using System;

namespace Ats.Domain.Booking
{
    public class DiscountOffer
    {
        public DiscountOffer(string name, Money value)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace", nameof(name));

            Name = name;
            Value = value;
        }

        public string Name { get; }
        public Money Value { get; }
    }
}
