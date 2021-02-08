namespace Ats.Domain.Airports
{
    public struct Airport
    {
        public Airport(AirportCode code, Continent continent)
        {
            Code = code;
            Continent = continent;
        }

        public AirportCode Code { get; }
        public Continent Continent { get; }
    }
}
