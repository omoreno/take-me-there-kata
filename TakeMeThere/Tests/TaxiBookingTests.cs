using System;
using Moq;
using NUnit.Framework;
using TakeMeThere.Exceptions;
using TakeMeThere.Models;
using TakeMeThere.Repositories;
using TakeMeThere.Services;

namespace TakeMeThere.Tests
{
    [TestFixture]
    public class TaxiBookingTests
    {
        private CommandLineInterface cli;
        private Mock<IBookingRepository> bookingRepository;
        private Mock<ITaxiRepository> availableTaxiRepository;
        private Taxi taxi;

        private Customer customer;

        [SetUp]
        public void SetUp()
        {
            availableTaxiRepository = new Mock<ITaxiRepository>();
            bookingRepository = new Mock<IBookingRepository>();
            var bookingService = new BookingService(availableTaxiRepository.Object, bookingRepository.Object);
            cli = new CommandLineInterface(bookingService, null, null, null);
            var taxiFeatures = new TaxiFeatures(TaxiSize.Small, 4, false, false, false, false);
            var preferences = new TaxiAvailabilityPreferences(TaxiTripLength.Short, 3, 10000);
            taxi = new Taxi(taxiFeatures, new Location(1, 1), preferences);
            customer = new Customer(new CustomerPreferences(null));
        }

        [Test]
        public void ShouldReturnBookingReferenceWhenNewBookingIsMade()
        {
            availableTaxiRepository
                    .Setup(x => x.Exists(It.IsAny<string>()))
                    .Returns(true);

            var bookingReference = cli.BookTaxi(taxi, customer, new Location(1, 1), new Location(1, 1), 150.3);

            Assert.IsNotNull(bookingReference);
        }

        [Test]
        public void ShouldThrowIfTaxiIsAlreadyBooked()
        {
            availableTaxiRepository
                    .Setup(x => x.Exists(It.IsAny<string>()))
                    .Returns(false);

            Action act = () => cli.BookTaxi(taxi, customer, new Location(1, 1), new Location(1, 1), 150.3);

            Assert.Throws(typeof(AlreadyBookedTaxi), act.Invoke);
        }

        [Test]
        public void ShouldSaveNewBooking()
        {
            availableTaxiRepository
                .Setup(x => x.Exists(It.IsAny<string>()))
                .Returns(true);

            cli.BookTaxi(taxi, customer, new Location(1, 1), new Location(1, 1), 150.3);

            bookingRepository.Verify(x => x.Save(It.IsAny<Booking>()));
        }

        [Test]
        public void ShouldSetTaxiUnavailableOnNewBooking()
        {
            availableTaxiRepository
                .Setup(x => x.Exists(It.IsAny<string>()))
                .Returns(true);

            cli.BookTaxi(taxi, customer, new Location(1, 1), new Location(1, 1), 150.3);

            availableTaxiRepository.Verify(x => x.Delete(It.IsAny<string>()));
        }

        [Test]
        public void CannotCancelBookingIfNotExists()
        {
            bookingRepository
                .Setup(x => x.FindByReference(It.IsAny<string>()))
                .Throws(new BookingNotExists());

            Action act = () => cli.CancelBooking("NotValidBookReference");

            Assert.Throws(typeof (BookingNotExists), act.Invoke);
        }

        [Test]
        public void CannotCancelBookingWhenCancellationTimeLimitReached()
        {
            bookingRepository
                .Setup(x => x.FindByReference(It.IsAny<string>()))
                .Returns(new Booking(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),
                            DateTime.Now.AddMinutes(-11)));

            Action act = () => cli.CancelBooking("bookReference");

            Assert.Throws(typeof(CancellationTimeLimitReached), act.Invoke);
        }

        [Test]
        public void ShouldCancelBooking()
        {
            bookingRepository
                .Setup(x => x.FindByReference(It.IsAny<string>()))
                .Returns(new Booking(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),
                            DateTime.Now.AddMinutes(-9)));

            cli.CancelBooking("bookReference");

            bookingRepository.Verify(x => x.Delete(It.IsAny<Booking>()));
        }
    }
}