using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using TakeMeThere.Models;
using TakeMeThere.Repositories;
using TakeMeThere.Services;

namespace TakeMeThere
{
    [TestFixture]
    public class TaxiFinderTests
    {
        private CommandLineInterface api;
        private Mock<IAvailableTaxiRepository> availableTaxiRepository;
        private TaxiFeatures taxiFeatures;
        private Customer customer;
        private TaxiAvailabilityPreferences taxiPreferences;
        private List<AvailableTaxi> availableTaxis;

        [SetUp]
        public void SetUp()
        {
            availableTaxiRepository = new Mock<IAvailableTaxiRepository>();
            var taxiFinder = new TaxiFinder(availableTaxiRepository.Object);
            api = new CommandLineInterface(null, null, taxiFinder);
            taxiFeatures = new TaxiFeatures(TaxiSize.Small, 4, false, false, false, false);
            customer = new Customer(new CustomerPreferences(null));
            taxiPreferences = new TaxiAvailabilityPreferences(TaxiTripLength.Short, null, 10000);
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
            
            Assert.AreEqual(10, retrievedTaxis.Count);
        }

        [Test]
        public void ShouldFilterMostAffordableTaxis()
        {
            var mostAffodableAvailableTaxi = new AvailableTaxi(taxiFeatures, new Location(1, 1), taxiPreferences);
            var mostExpensiveTaxi = new TaxiFeatures(TaxiSize.Large, 7, true, true, true, true);
            var mostExpensiveAvailableTaxi = new AvailableTaxi(mostExpensiveTaxi, new Location(1, 1), taxiPreferences);
            availableTaxis.Add(mostExpensiveAvailableTaxi);
            availableTaxis.Add(mostAffodableAvailableTaxi);

            var retrievedTaxis = api.GetTaxis(customer, new Location(1, 1), TaxiSearchFilter.MostAffordable, null);

            Assert.AreEqual(2, retrievedTaxis.Count);
            Assert.Less(retrievedTaxis.First().Features.Price, retrievedTaxis.Last().Features.Price);
        }

        [Test]
        public void ShouldFilterNearestTaxis()
        {
            var farthestAvailableTaxi = new AvailableTaxi(taxiFeatures, new Location(2, 2), taxiPreferences);
            var customerLocation = new Location(1, 1);
            var closerTaxi = new TaxiFeatures(TaxiSize.Small, 4, false, false, false, false);
            var closerAvailableTaxi = new AvailableTaxi(closerTaxi, new Location(1, 1), taxiPreferences);
            availableTaxis.Add(farthestAvailableTaxi);
            availableTaxis.Add(closerAvailableTaxi);

            var retrievedTaxis = api.GetTaxis(customer, customerLocation, TaxiSearchFilter.Nearest, new CustomerNeeds(TaxiSize.Small, 4, false, false, false, false));

            Assert.AreEqual(2, retrievedTaxis.Count);
            Assert.Less(retrievedTaxis.First().DistanceToCustomer(customerLocation), retrievedTaxis.Last().DistanceToCustomer(customerLocation));
        }

        [Test]
        public void ShouldFilterBestRatedTaxis()
        {
            var worstRatedTaxi = new AvailableTaxi(taxiFeatures, new Location(1, 1), taxiPreferences);
            worstRatedTaxi.Rate(0);
            var bestRatedTaxi = new AvailableTaxi(taxiFeatures, new Location(1, 1), taxiPreferences);
            bestRatedTaxi.Rate(5);
            availableTaxis.Add(worstRatedTaxi);
            availableTaxis.Add(bestRatedTaxi);

            var retrievedTaxis = api.GetTaxis(customer, new Location(1, 1), TaxiSearchFilter.BestRated, new CustomerNeeds(TaxiSize.Small, 4, false, false, false, false));

            Assert.AreEqual(2, retrievedTaxis.Count);
            Assert.Greater(retrievedTaxis.First().Rating.Value, retrievedTaxis.Last().Rating.Value);
        }

        [Test]
        public void ShouldFilterTaxisThatMatchCustomerTaxiSizeNeeds()
        {
            var notMatchingTaxi = new AvailableTaxi(taxiFeatures, new Location(2, 2), taxiPreferences);
            availableTaxis.Add(notMatchingTaxi);
            var customerNeeds = new CustomerNeeds(TaxiSize.Large, 4, false, false, false, false);

            var retrievedTaxis = api.GetTaxis(customer, new Location(1, 1), TaxiSearchFilter.Nearest, customerNeeds);

            Assert.IsEmpty(retrievedTaxis);
        }

        [Test]
        public void ShouldFilterTaxisThatMatchCustomerNumberOfSeatsNeeds()
        {
            var notMatchingTaxi = new AvailableTaxi(taxiFeatures, new Location(2, 2), taxiPreferences);
            availableTaxis.Add(notMatchingTaxi);
            var customerNeeds = new CustomerNeeds(TaxiSize.Small, 7, false, false, false, false);

            var retrievedTaxis = api.GetTaxis(customer, new Location(1, 1), TaxiSearchFilter.Nearest, customerNeeds);

            Assert.IsEmpty(retrievedTaxis);
        }

        [Test]
        public void ShouldFilterTaxisThatMatchCustomerAirConditionedNeeds()
        {
            var notMatchingTaxi = new AvailableTaxi(taxiFeatures, new Location(2, 2), taxiPreferences);
            availableTaxis.Add(notMatchingTaxi);
            var customerNeeds = new CustomerNeeds(TaxiSize.Small, 4, true, false, false, false);

            var retrievedTaxis = api.GetTaxis(customer, new Location(1, 1), TaxiSearchFilter.Nearest, customerNeeds);

            Assert.IsEmpty(retrievedTaxis);
        }

        [Test]
        public void ShouldFilterTaxisThatMatchCustomerWheelchairAccesibilityNeeds()
        {
            var notMatchingTaxi = new AvailableTaxi(taxiFeatures, new Location(2, 2), taxiPreferences);
            availableTaxis.Add(notMatchingTaxi);
            var customerNeeds = new CustomerNeeds(TaxiSize.Small, 4, false, true, false, false);

            var retrievedTaxis = api.GetTaxis(customer, new Location(1, 1), TaxiSearchFilter.Nearest, customerNeeds);

            Assert.IsEmpty(retrievedTaxis);
        }

        [Test]
        public void ShouldFilterTaxisThatMatchCustomerExtraBaggageSpaceNeeds()
        {
            var notMatchingTaxi = new AvailableTaxi(taxiFeatures, new Location(2, 2), taxiPreferences);
            availableTaxis.Add(notMatchingTaxi);
            var customerNeeds = new CustomerNeeds(TaxiSize.Small, 4, false, false, true, false);

            var retrievedTaxis = api.GetTaxis(customer, new Location(1, 1), TaxiSearchFilter.Nearest, customerNeeds);

            Assert.IsEmpty(retrievedTaxis);
        }

        [Test]
        public void ShouldFilterTaxisThatMatchCustomerLuxuriousEquipmentNeeds()
        {
            var notMatchingTaxi = new AvailableTaxi(taxiFeatures, new Location(2, 2), taxiPreferences);
            availableTaxis.Add(notMatchingTaxi);
            var customerNeeds = new CustomerNeeds(TaxiSize.Small, 4, false, false, false, true);

            var retrievedTaxis = api.GetTaxis(customer, new Location(1, 1), TaxiSearchFilter.Nearest, customerNeeds);

            Assert.IsEmpty(retrievedTaxis);
        }

        [Test]
        public void ShouldFilterTaxisThatMatchCustomerMinimunRating()
        {
            var customerPreferences = new CustomerPreferences(taxiMinimunRating: 4);
            customer = new Customer(customerPreferences);
            var notMatchingTaxi = new AvailableTaxi(taxiFeatures, new Location(2, 2), taxiPreferences);
            availableTaxis.Add(notMatchingTaxi);
            var customerNeeds = new CustomerNeeds(TaxiSize.Small, 4, false, false, false, false);

            var retrievedTaxis = api.GetTaxis(customer, new Location(1, 1), TaxiSearchFilter.Nearest, customerNeeds);

            Assert.IsEmpty(retrievedTaxis);
        }

        [Test]
        public void ShouldFilterTaxisThatMatchTaxiOwnerMinimunRating()
        {
            taxiPreferences = new TaxiAvailabilityPreferences(TaxiTripLength.Long, 4, 10000);
            var notMatchingTaxi = new AvailableTaxi(taxiFeatures, new Location(2, 2), taxiPreferences);
            availableTaxis.Add(notMatchingTaxi);
            var customerNeeds = new CustomerNeeds(TaxiSize.Small, 4, false, false, false, false);

            var retrievedTaxis = api.GetTaxis(customer, new Location(1, 1), TaxiSearchFilter.Nearest, customerNeeds);

            Assert.IsEmpty(retrievedTaxis);
        }

        [Test]
        public void ShouldFilterTaxisThatAreOutsideWorkingLocationRadio()
        {
            taxiPreferences = new TaxiAvailabilityPreferences(TaxiTripLength.Short, null, 10);
            var notMatchingTaxi = new AvailableTaxi(taxiFeatures, new Location(2, 2), taxiPreferences);
            availableTaxis.Add(notMatchingTaxi);
            var customerNeeds = new CustomerNeeds(TaxiSize.Small, 4, false, false, false, false);

            var retrievedTaxis = api.GetTaxis(customer, new Location(100, 100), TaxiSearchFilter.Nearest, customerNeeds);

            Assert.IsEmpty(retrievedTaxis);
        }

        private List<AvailableTaxi> GetStubTaxis(int numberOfTaxis)
        {
            var taxis = new List<AvailableTaxi>();
            for (var i = 0; i < numberOfTaxis; i++)
                taxis.Add(new AvailableTaxi(taxiFeatures, new Location(1, 1), taxiPreferences));
            return taxis;
        }
    }

    [TestFixture]
    public class RatingServiceTests
    {
        [Test]
        public void TaxiOwnerCanRateCustomer()
        {
            var customerRepository = new Mock<ICustomerRepository>();
            var ratingService = new RatingService(customerRepository.Object, null);
            var taxi = new AvailableTaxi(new TaxiFeatures(TaxiSize.Large, 4, false, false, false, false),
                                         new Location(1, 1),
                                         new TaxiAvailabilityPreferences(TaxiTripLength.Long, null, 10000));
            var customer = new Customer(new CustomerPreferences(null));

            ratingService.RateCustomer(taxi, customer, 0);

            Assert.AreEqual(0, customer.Rating.Value);
            customerRepository.Verify(x => x.Update(It.IsAny<Customer>()));
        }

        [Test]
        public void CustomerCanCanRateTaxi()
        {
            var taxiRepository = new Mock<IAvailableTaxiRepository>();
            var ratingService = new RatingService(null, taxiRepository.Object);
            var taxi = new AvailableTaxi(new TaxiFeatures(TaxiSize.Large, 4, false, false, false, false),
                                         new Location(1, 1),
                                         new TaxiAvailabilityPreferences(TaxiTripLength.Long, null, 10000));
            var customer = new Customer(new CustomerPreferences(null));

            ratingService.RateTaxi(customer, taxi, 0);

            Assert.AreEqual(0, taxi.Rating.Value);
            taxiRepository.Verify(x => x.Update(It.IsAny<AvailableTaxi>()));
        }
    }

    public interface ICustomerRepository
    {
        void Update(Customer customer);
    }

    public class RatingService
    {
        private readonly ICustomerRepository customerRepository;
        private readonly IAvailableTaxiRepository availableTaxiRepository;

        public RatingService(ICustomerRepository customerRepository, IAvailableTaxiRepository availableTaxiRepository)
        {
            this.customerRepository = customerRepository;
            this.availableTaxiRepository = availableTaxiRepository;
        }

        public void RateCustomer(AvailableTaxi taxi, Customer customer, int rate)
        {
            customer.Rate(rate);
            customerRepository.Update(customer);
        }

        public void RateTaxi(Customer customer, AvailableTaxi taxi, int rate)
        {
            taxi.Rate(rate);
            availableTaxiRepository.Update(taxi);
        }
    }
}