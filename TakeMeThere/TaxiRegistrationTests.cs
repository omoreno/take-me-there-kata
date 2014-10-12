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
            var api = new Api(availableTaxiRepository.Object);
            var taxi = new Taxi(size: TaxiSize.Small, numberOfSeats: 4, airConditioned: true,
                                wheelchairAccesible: false, extraBaggageSpace: false,
                                luxuriousEquipment: false);
            var taxiAvailabilityPreferences = new TaxiAvailabilityPreferences(TaxiTripLength.Short, 3, 10000);

            api.RegisterTaxi(taxi, new Location(0, 0), taxiAvailabilityPreferences);

            availableTaxiRepository.Verify(x => x.Save(It.IsAny<AvailableTaxi>()));
        }
    }

    [TestFixture]
    public class TaxiBookingTests
    {
        [Test]
        public void ShouldReturnBookingReferenceWhenNewBookingIsMade()
        {
            var availableTaxiRepository = new Mock<IAvailableTaxiRepository>();
            var api = new Api(availableTaxiRepository.Object);
            var taxi = new Taxi(TaxiSize.Small, 4, true, false, false, false);
            var customer = new Customer();

            var bookingReference = api.BookTaxi(taxi, customer, new Location(1, 1), new Location(1, 1), 150.3);

            Assert.IsNotNull(bookingReference);
        }
    }

    public class Customer
    {
    }
}
