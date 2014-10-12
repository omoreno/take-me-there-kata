using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using TakeMeThere.Models;
using TakeMeThere.Repositories;

namespace TakeMeThere
{
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

        [Test]
        public void ShouldFilterMostAffordableTaxis()
        {
            var preferences = new TaxiAvailabilityPreferences(TaxiTripLength.Short, 3, 10000);
            var mostAffodableAvailableTaxi = new AvailableTaxi(taxi, new Location(1, 1), preferences);
            var mostExpensiveTaxi = new Taxi(TaxiSize.Large, 8, true, true, true, true);
            var mostExpensiveAvailableTaxi = new AvailableTaxi(mostExpensiveTaxi, new Location(1, 1), preferences);
            var availableTaxis = new List<AvailableTaxi> { mostExpensiveAvailableTaxi, mostAffodableAvailableTaxi };
            availableTaxiRepository
                .Setup(x => x.GetAll())
                .Returns(availableTaxis);

            var retrievedTaxis = api.GetTaxis(customer, new Location(1, 1), TaxiSearchFilter.MostAffordable, new CustomerNeeds(TaxiSize.Small, 4, false, false, false, false));

            Assert.AreEqual(retrievedTaxis.Count, 2);
            Assert.Less(retrievedTaxis.First().Price, retrievedTaxis.Last().Price);
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