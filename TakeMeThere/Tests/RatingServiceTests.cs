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
    public class RatingServiceTests
    {
        private Mock<ICustomerRepository> customerRepository;
        private RatingService ratingService;
        private AvailableTaxi taxi;
        private Customer customer;
        private Mock<IAvailableTaxiRepository> taxiRepository;

        [SetUp]
        public void SetUp()
        {
            customerRepository = new Mock<ICustomerRepository>();
            taxiRepository = new Mock<IAvailableTaxiRepository>();
            ratingService = new RatingService(customerRepository.Object, taxiRepository.Object);
            taxi = new AvailableTaxi(new TaxiFeatures(TaxiSize.Large, 4, false, false, false, false),
                                     new Location(1, 1),
                                     new TaxiAvailabilityPreferences(TaxiTripLength.Long, null, 10000));
            customer = new Customer(new CustomerPreferences(null));

        }

        [Test]
        public void TaxiOwnerCanRateCustomer()
        {

            ratingService.RateCustomer(taxi, customer, 1);

            Assert.AreEqual(1, customer.Rating.Value);
            customerRepository.Verify(x => x.Update(It.IsAny<Customer>()));
        }

        [Test]
        public void CustomerCanCanRateTaxi()
        {

            ratingService.RateTaxi(customer, taxi, 1);

            Assert.AreEqual(1, taxi.Rating.Value);
            taxiRepository.Verify(x => x.Update(It.IsAny<AvailableTaxi>()));
        }

        [Test]
        public void TaxiRatingShouldBeGreaterOrEqualThanOne()
        {

            Action act = () => ratingService.RateTaxi(customer, taxi, 0);

            Assert.Throws<NotValidRating>(act.Invoke);
        }

        [Test]
        public void TaxiRatingShouldBeLessOrEqualThanFive()
        {

            Action act = () => ratingService.RateTaxi(customer, taxi, 6);

            Assert.Throws<NotValidRating>(act.Invoke);
        }

        [Test]
        public void CustomerRatingShouldBeGreaterOrEqualThanOne()
        {

            Action act = () => ratingService.RateCustomer(taxi, customer, 0);

            Assert.Throws<NotValidRating>(act.Invoke);
        }

        [Test]
        public void CustomerRatingShouldBeLessOrEqualThanFive()
        {

            Action act = () => ratingService.RateCustomer(taxi,customer, 6);

            Assert.Throws<NotValidRating>(act.Invoke);
        }

        [Test]
        public void CustomerRatingShouldBeTheAverageOfAllRatings()
        {
            
            ratingService.RateCustomer(taxi, customer, 1);
            ratingService.RateCustomer(taxi, customer, 5);

            Assert.AreEqual(3, customer.Rating);
        }

        [Test]
        public void TaxiRatingShouldBeTheAverageOfAllRatings()
        {

            ratingService.RateTaxi(customer,taxi, 1);
            ratingService.RateTaxi(customer, taxi, 5);

            Assert.AreEqual(3, taxi.Rating);
        }
    }
}