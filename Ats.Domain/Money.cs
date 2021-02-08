namespace Ats.Domain
{
    public struct Money
    {
        private readonly decimal _value;

        private Money(decimal value)
        {
            _value = value;
        }

        public static implicit operator decimal(Money money) => money._value;
        public static implicit operator Money(decimal value) => new Money(value);

        public static Money operator +(Money lhs, Money rhs)
        {
            return lhs._value + rhs._value;
        }

        public static Money operator -(Money lhs, Money rhs)
        {
            return lhs._value - rhs._value;
        }
    }
}
