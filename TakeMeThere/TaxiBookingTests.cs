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
        private CommandLineInterface cli;
        private Mock<IBookingRepository> bookingRepository;
        private Mock<IAvailableTaxiRepository> availableTaxiRepository;
        private AvailableTaxi availableTaxi;

        private Customer customer;

        [SetUp]
        public void SetUp()
        {
            availableTaxiRepository = new Mock<IAvailableTaxiRepository>();
            bookingRepository = new Mock<IBookingRepository>();
            var bookingService = new BookingService(availableTaxiRepository.Object, bookingRepository.Object);
            cli = new CommandLineInterface(bookingService, null, null);
            var taxiFeatures = new TaxiFeatures(TaxiSize.Small, 4, false, false, false, false);
            var preferences = new TaxiAvailabilityPreferences(TaxiTripLength.Short, 3, 10000);
            availableTaxi = new AvailableTaxi(taxiFeatures, new Location(1, 1), preferences);
            customer = new Customer();
        }

        [Test]
        public void ShouldReturnBookingReferenceWhenNewBookingIsMade()
        {
            availableTaxiRepository
                    .Setup(x => x.Exists(It.IsAny<string>()))
                    .Returns(true);

            var bookingReference = cli.BookTaxi(availableTaxi, customer, new Location(1, 1), new Location(1, 1), 150.3);

            Assert.IsNotNull(bookingReference);
        }

        [Test]
        public void ShouldThrowIfTaxiIsAlreadyBooked()
        {
            availableTaxiRepository
                    .Setup(x => x.Exists(It.IsAny<string>()))
                    .Returns(false);

            Action act = () => cli.BookTaxi(availableTaxi, customer, new Location(1, 1), new Location(1, 1), 150.3);

            Assert.Throws(typeof(AlreadyBookedTaxi), act.Invoke);
        }

        [Test]
        public void ShouldSaveNewBooking()
        {
            availableTaxiRepository
                .Setup(x => x.Exists(It.IsAny<string>()))
                .Returns(true);

            cli.BookTaxi(availableTaxi, customer, new Location(1, 1), new Location(1, 1), 150.3);

            bookingRepository.Verify(x => x.Save(It.IsAny<BookingRequest>()));
        }

        [Test]
        public void ShouldSetTaxiUnavailableOnNewBooking()
        {
            availableTaxiRepository
                .Setup(x => x.Exists(It.IsAny<string>()))
                .Returns(true);

            cli.BookTaxi(availableTaxi, customer, new Location(1, 1), new Location(1, 1), 150.3);

            availableTaxiRepository.Verify(x => x.Delete(It.IsAny<string>()));
        }
    }
}