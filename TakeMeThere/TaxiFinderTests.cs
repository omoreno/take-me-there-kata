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
        private TaxiFeatures taxiFeatures;
        private Customer customer;
        private TaxiAvailabilityPreferences preferences;
        private List<AvailableTaxi> availableTaxis;

        [SetUp]
        public void SetUp()
        {
            availableTaxiRepository = new Mock<IAvailableTaxiRepository>();
            bookingRepository = new Mock<IBookingRepository>();
            api = new Api(availableTaxiRepository.Object, bookingRepository.Object);
            taxiFeatures = new TaxiFeatures(TaxiSize.Small, 4, false, false, false, false);
            customer = new Customer();
            preferences = new TaxiAvailabilityPreferences(TaxiTripLength.Short, 3, 10000);
            availableTaxis = new List<AvailableTaxi>();
            availableTaxiRepository
                .Setup(x => x.GetAll())
                .Returns(availableTaxis);
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
            var mostAffodableAvailableTaxi = new AvailableTaxi(taxiFeatures, new Location(1, 1), preferences);
            var mostExpensiveTaxi = new TaxiFeatures(TaxiSize.Small, 4, true, true, true, true);
            var mostExpensiveAvailableTaxi = new AvailableTaxi(mostExpensiveTaxi, new Location(1, 1), preferences);
            availableTaxis.Add(mostExpensiveAvailableTaxi);
            availableTaxis.Add(mostAffodableAvailableTaxi);

            var retrievedTaxis = api.GetTaxis(customer, new Location(1, 1), TaxiSearchFilter.MostAffordable, new CustomerNeeds(TaxiSize.Small, 4, false, false, false, false));

            Assert.AreEqual(retrievedTaxis.Count, 2);
            Assert.Less(retrievedTaxis.First().Features.Price, retrievedTaxis.Last().Features.Price);
        }

        [Test]
        public void ShouldFilterNearestTaxis()
        {
            var farthestAvailableTaxi = new AvailableTaxi(taxiFeatures, new Location(2, 2), preferences);
            var customerLocation = new Location(1, 1);
            var closerTaxi = new TaxiFeatures(TaxiSize.Small, 4, true, true, true, true);
            var closerAvailableTaxi = new AvailableTaxi(closerTaxi, new Location(1, 1), preferences);
            availableTaxis.Add(farthestAvailableTaxi);
            availableTaxis.Add(closerAvailableTaxi);

            var retrievedTaxis = api.GetTaxis(customer, customerLocation, TaxiSearchFilter.Nearest, new CustomerNeeds(TaxiSize.Small, 4, false, false, false, false));

            Assert.AreEqual(retrievedTaxis.Count, 2);
            Assert.Less(retrievedTaxis.First().DistanceToCustomer(customerLocation), retrievedTaxis.Last().DistanceToCustomer(customerLocation));
        }

        [Test]
        public void ShouldFilterTaxisThatMatchCustomerTaxiSizeNeeds()
        {
            var notMatchingTaxi = new AvailableTaxi(taxiFeatures, new Location(2, 2), preferences);
            availableTaxis.Add(notMatchingTaxi);
            var customerNeeds = new CustomerNeeds(TaxiSize.Large, 4, false, false, false, false);

            var retrievedTaxis = api.GetTaxis(customer, new Location(1, 1), TaxiSearchFilter.Nearest, customerNeeds);

            Assert.IsEmpty(retrievedTaxis);
        }

        [Test]
        public void ShouldFilterTaxisThatMatchCustomerNumberOfSeatsNeeds()
        {
            var notMatchingTaxi = new AvailableTaxi(taxiFeatures, new Location(2, 2), preferences);
            availableTaxis.Add(notMatchingTaxi);
            var customerNeeds = new CustomerNeeds(TaxiSize.Small, 7, false, false, false, false);

            var retrievedTaxis = api.GetTaxis(customer, new Location(1, 1), TaxiSearchFilter.Nearest, customerNeeds);

            Assert.IsEmpty(retrievedTaxis);
        }

        private List<AvailableTaxi> GetStubTaxis(int numberOfTaxis)
        {
            var taxis = new List<AvailableTaxi>();
            for (var i = 0; i < numberOfTaxis; i++)
                taxis.Add(new AvailableTaxi(taxiFeatures, new Location(1, 1), preferences));
            return taxis;
        }

    }
}