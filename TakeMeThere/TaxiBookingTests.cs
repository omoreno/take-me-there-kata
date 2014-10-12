using System;
using Moq;
using NUnit.Framework;
using TakeMeThere.Exceptions;
using TakeMeThere.Models;
using TakeMeThere.Repositories;

namespace TakeMeThere
{
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
            availableTaxiRepository
                    .Setup(x => x.Exists(It.IsAny<string>()))
                    .Returns(true);

            var bookingReference = api.BookTaxi(taxi, customer, new Location(1, 1), new Location(1, 1), 150.3);

            Assert.IsNotNull(bookingReference);
        }

        [Test]
        public void ShouldThrowIfTaxiIsAlreadyBooked()
        {
            var availableTaxiRepository = new Mock<IAvailableTaxiRepository>();
            var api = new Api(availableTaxiRepository.Object);
            var taxi = new Taxi(TaxiSize.Small, 4, true, false, false, false);
            var customer = new Customer();
            availableTaxiRepository
                    .Setup(x => x.Exists(It.IsAny<string>()))
                    .Returns(false);

            Action act = () => api.BookTaxi(taxi, customer, new Location(1, 1), new Location(1, 1), 150.3);

            Assert.Throws(typeof(AlreadyBookedTaxi), act.Invoke);
        }
    }
}