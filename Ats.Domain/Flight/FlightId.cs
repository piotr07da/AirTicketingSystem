using System.Text.RegularExpressions;

namespace Ats.Domain.Flight
{
    public struct FlightId
    {
        private static readonly Regex _parserRx = new Regex("(?<airlineDesignator>[A-Za-z]{3})\\s(?<flightNumber>\\d{5})\\s(?<unknownSuffix>[A-Za-z]{3})");

        private readonly string _airlineDesignator;
        private readonly int _flightNumber;
        private readonly string _unknownSuffix;

        private FlightId(string airlineDesignator, int flightNumber, string unknownSuffix)
        {
            _airlineDesignator = airlineDesignator;
            _flightNumber = flightNumber;
            _unknownSuffix = unknownSuffix;
        }

        public override string ToString()
        {
            return $"{_airlineDesignator} {_flightNumber} {_unknownSuffix}";
        }

        public static FlightId Parse(string flightId)
        {
            var m = _parserRx.Match(flightId);

            if (!m.Success)
            {
                throw new DomainLogicException($"Flight id {flightId} is incorrect. Correct flight id consists of: 3 letter airline designator, 5 letter flight number, 3 letter suffix of unkonwn purpose.");
            }

            var adg = m.Groups["airlineDesignator"];
            var fng = m.Groups["flightNumber"];
            var usg = m.Groups["unknownSuffix"];

            return new FlightId(adg.Value, int.Parse(fng.Value), usg.Value);
        }

        public static implicit operator string(FlightId flightId) => flightId.ToString();
        public static implicit operator FlightId(string flightId) => Parse(flightId);
    }
}
