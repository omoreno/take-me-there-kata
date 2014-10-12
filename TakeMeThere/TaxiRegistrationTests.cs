using Moq;
using NUnit.Framework;
using TakeMeThere.Models;
using TakeMeThere.Repositories;

namespace TakeMeThere
{
    [TestFixture]
    public class TaxiRegistrationTests
    {
        [Test]
        public void ShouldRegisterNewAvailableTaxi()
        {
            var availableTaxiRepository = new Mock<IAvailableTaxiRepository>();
            var api = new Api(availableTaxiRepository.Object, null);
            var taxi = new Taxi(size: TaxiSize.Small, numberOfSeats: 4, airConditioned: true,
                                wheelchairAccesible: false, extraBaggageSpace: false,
                                luxuriousEquipment: false);
            var taxiAvailabilityPreferences = new TaxiAvailabilityPreferences(TaxiTripLength.Short, 3, 10000);

            api.RegisterTaxi(taxi, new Location(0, 0), taxiAvailabilityPreferences);

            availableTaxiRepository.Verify(x => x.Save(It.IsAny<AvailableTaxi>()));
        }
    }
}
