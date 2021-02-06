namespace Ats.Domain
{
    public class Money
    {
        private readonly decimal _value;

        private Money(decimal value)
        {
            _value = value;
        }

        public static implicit operator decimal(Money money) => money._value;
        public static implicit operator Money(decimal value) => new Money(value);
    }
}
