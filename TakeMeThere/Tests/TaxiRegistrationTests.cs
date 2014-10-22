using Moq;
using NUnit.Framework;
using TakeMeThere.Models;
using TakeMeThere.Repositories;
using TakeMeThere.Services;

namespace TakeMeThere.Tests
{
    [TestFixture]
    public class TaxiRegistrationTests
    {
        [Test]
        public void ShouldRegisterNewAvailableTaxi()
        {
            var availableTaxiRepository = new Mock<ITaxiRepository>();
            var registerService = new TaxiRegisterService(availableTaxiRepository.Object);
            var api = new CommandLineInterface(null, registerService, null, null);
            var taxi = new TaxiFeatures(size: TaxiSize.Small, numberOfSeats: 4, airConditioned: true,
                                wheelchairAccesible: false, extraBaggageSpace: false,
                                luxuriousEquipment: false);
            var taxiAvailabilityPreferences = new TaxiAvailabilityPreferences(TaxiTripLength.Short, 3, 10000);

            api.RegisterTaxi(taxi, new Location(0, 0), taxiAvailabilityPreferences);

            availableTaxiRepository.Verify(x => x.Save(It.IsAny<Taxi>()));
        }
    }
}
