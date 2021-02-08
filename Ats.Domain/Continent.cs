using System.Linq;

namespace Ats.Domain
{
    public struct Continent
    {
        public static readonly Continent Africa = new Continent("Africa");
        public static readonly Continent Antractica = new Continent("Antractica");
        public static readonly Continent Asia = new Continent("Asia");
        public static readonly Continent Australia = new Continent("Australia");
        public static readonly Continent Europe = new Continent("Europe");
        public static readonly Continent NorthAmerica = new Continent("NorthAmerica");
        public static readonly Continent SouthAmerica = new Continent("SouthAmerica");

        private static readonly Continent[] _allContinents = new Continent[]
        {
            Africa,
            Antractica,
            Asia,
            Australia,
            Europe,
            NorthAmerica,
            SouthAmerica,
        };

        private readonly string _continentName;

        private Continent(string continentName)
        {
            _continentName = continentName;
        }

        public override string ToString() => _continentName;

        public override bool Equals(object obj)
        {
            return _continentName.Equals(((Continent)obj)._continentName);
        }

        public override int GetHashCode()
        {
            return _continentName.GetHashCode();
        }

        public static Continent Parse(string continentName)
        {
            var continent = new Continent(continentName);

            if (!_allContinents.Contains(continent))
            {
                throw new DomainLogicException($"Continent name {continentName} is incorrect.");
            }

            return continent;
        }

        public static implicit operator string(Continent code) => code._continentName;
        public static implicit operator Continent(string code) => Parse(code);
    }
}
