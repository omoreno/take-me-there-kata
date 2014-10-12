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
            var api = new Api(availableTaxiRepository.Object, null);
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
            var api = new Api(availableTaxiRepository.Object, null);
            var taxi = new Taxi(TaxiSize.Small, 4, true, false, false, false);
            var customer = new Customer();
            availableTaxiRepository
                    .Setup(x => x.Exists(It.IsAny<string>()))
                    .Returns(false);

            Action act = () => api.BookTaxi(taxi, customer, new Location(1, 1), new Location(1, 1), 150.3);

            Assert.Throws(typeof(AlreadyBookedTaxi), act.Invoke);
        }

        [Test]
        public void ShouldSaveNewBooking()
        {
            var bookingRepository = new Mock<IBookingRepository>();
            var availableTaxiRepository = new Mock<IAvailableTaxiRepository>();
            var api = new Api(availableTaxiRepository.Object, bookingRepository.Object);
            var taxi = new Taxi(TaxiSize.Small, 4, true, false, false, false);
            availableTaxiRepository
                .Setup(x => x.Exists(It.IsAny<string>()))
                .Returns(true);
            var customer = new Customer();

            api.BookTaxi(taxi, customer, new Location(1, 1), new Location(1, 1), 150.3);

            bookingRepository.Verify(x => x.Save(It.IsAny<BookingRequest>()));
        }
    }

    public class BookingRequest
    {
        public string Id { get; private set;  }
        private readonly string taxiId;
        private readonly string customerId;

        public BookingRequest(string taxiId, string customerId)
        {
            Id = new Guid().ToString();
            this.taxiId = taxiId;
            this.customerId = customerId;
        }
    }

    public interface IBookingRepository
    {
        void Save(BookingRequest bookingRequest);
    }
}