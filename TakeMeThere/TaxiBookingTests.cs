using System;
using System.Collections.Generic;
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
        private Api api;
        private Mock<IBookingRepository> bookingRepository;
        private Mock<IAvailableTaxiRepository> availableTaxiRepository;
        private Taxi taxi;
        private Customer customer;

        [SetUp]
        public void SetUp()
        {
            availableTaxiRepository = new Mock<IAvailableTaxiRepository>();
            bookingRepository = new Mock<IBookingRepository>();
            api = new Api(availableTaxiRepository.Object, bookingRepository.Object);
            taxi = new Taxi(TaxiSize.Small, 4, true, false, false, false);
            customer = new Customer();
        }

        [Test]
        public void ShouldReturnBookingReferenceWhenNewBookingIsMade()
        {
            availableTaxiRepository
                    .Setup(x => x.Exists(It.IsAny<string>()))
                    .Returns(true);

            var bookingReference = api.BookTaxi(taxi, customer, new Location(1, 1), new Location(1, 1), 150.3);

            Assert.IsNotNull(bookingReference);
        }

        [Test]
        public void ShouldThrowIfTaxiIsAlreadyBooked()
        {
            availableTaxiRepository
                    .Setup(x => x.Exists(It.IsAny<string>()))
                    .Returns(false);

            Action act = () => api.BookTaxi(taxi, customer, new Location(1, 1), new Location(1, 1), 150.3);

            Assert.Throws(typeof(AlreadyBookedTaxi), act.Invoke);
        }

        [Test]
        public void ShouldSaveNewBooking()
        {
            availableTaxiRepository
                .Setup(x => x.Exists(It.IsAny<string>()))
                .Returns(true);

            api.BookTaxi(taxi, customer, new Location(1, 1), new Location(1, 1), 150.3);

            bookingRepository.Verify(x => x.Save(It.IsAny<BookingRequest>()));
        }

        [Test]
        public void ShouldSetTaxiUnavailableOnNewBooking()
        {
            availableTaxiRepository
                .Setup(x => x.Exists(It.IsAny<string>()))
                .Returns(true);

            api.BookTaxi(taxi, customer, new Location(1, 1), new Location(1, 1), 150.3);

            availableTaxiRepository.Verify(x => x.Delete(It.IsAny<string>()));
        }
    }

    [TestFixture]
    public class TaxiFinderTests
    {
        private Api api;
        private Mock<IBookingRepository> bookingRepository;
        private Mock<IAvailableTaxiRepository> availableTaxiRepository;
        private Taxi taxi;
        private Customer customer;

        [SetUp]
        public void SetUp()
        {
            availableTaxiRepository = new Mock<IAvailableTaxiRepository>();
            bookingRepository = new Mock<IBookingRepository>();
            api = new Api(availableTaxiRepository.Object, bookingRepository.Object);
            taxi = new Taxi(TaxiSize.Small, 4, false, false, false, false);
            customer = new Customer();
        }

        [Test]
        public void ShouldLimitResultSizeToTenTaxis()
        {
            availableTaxiRepository
                .Setup(x => x.GetAll())
                .Returns(GetStubTaxis(11));

            var retrievedTaxis = api.GetTaxis(customer, new Location(1, 1), TaxiSearchFilter.BestRated, new CustomerNeeds(TaxiSize.Small, 4, false, false, false, false));
            
            Assert.AreEqual(retrievedTaxis.Count, 10);
        }

        private List<AvailableTaxi> GetStubTaxis(int numberOfTaxis)
        {
            var preferences = new TaxiAvailabilityPreferences(TaxiTripLength.Short, 3, 10000);
            var taxis = new List<AvailableTaxi>();
            for (var i = 0; i < numberOfTaxis; i++)
                taxis.Add(new AvailableTaxi(taxi, new Location(1, 1), preferences));
            return taxis;
        }

    }
}