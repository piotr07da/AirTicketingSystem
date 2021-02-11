using Ats.Application.Airports;
using Ats.Domain.Airports;
using Ats.Tests.TestTools;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Ats.Tests.Logic.Airports
{
    public class given_no_airport_in_Airports
    {
        private GivenWhenThen _gwt;

        [SetUp]
        public void given()
        {
            _gwt = new GivenWhenThen();
        }

        [Test]
        public async Task when_AddAirport_then_Airport_added()
        {
            await Tester.TestAsync(_gwt
                .When(new AddAirportCommand(GlobalAirportsId.Id, 0, "KUL", "Asia"))
                .ThenIdentical(GlobalAirportsId.Id, new AirportAddedEvent(GlobalAirportsId.Id, "KUL", "Asia"))
            );
        }
    }
}
