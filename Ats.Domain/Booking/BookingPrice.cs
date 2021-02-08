namespace Ats.Domain.Booking
{
    public class BookingPrice
    {
        private static readonly Money _minValue = 20.00m;

        private Money _value;

        public BookingPrice(Money value)
        {
            _value = value;
        }

        public Money Value => _value;

        public void Decrease(Money decreaseValue)
        {
            _value -= decreaseValue;
            if (_value < _minValue)
            {
                _value = _minValue;
            }
        }
    }
}
