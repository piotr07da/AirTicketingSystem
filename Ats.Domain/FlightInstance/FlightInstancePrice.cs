namespace Ats.Domain.FlightInstance
{
    public class FlightInstancePrice
    {
        private Money _value;

        public FlightInstancePrice(Money value)
        {
            _value = value;
        }

        public Money Value => _value;
    }
}
