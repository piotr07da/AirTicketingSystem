using System.Text.RegularExpressions;

namespace Ats.Domain.Airports
{
    public struct AirportCode
    {
        private static readonly Regex _parserRx = new Regex("[a-zA-Z]{3}");

        private readonly string _code;

        private AirportCode(string code)
        {
            _code = code;
        }

        public override string ToString() => _code;

        public static AirportCode Parse(string code)
        {
            var m = _parserRx.Match(code);

            if (!m.Success)
            {
                throw new DomainLogicException($"Code {code} is incorrect. Correct airport code consists of 3 letters.");
            }

            return new AirportCode(m.Value);
        }

        public static implicit operator string(AirportCode code) => code._code;
        public static implicit operator AirportCode(string code) => Parse(code);
    }
}
