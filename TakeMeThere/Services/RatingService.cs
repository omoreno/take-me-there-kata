using TakeMeThere.Models;
using TakeMeThere.Repositories;

namespace TakeMeThere.Services
{
    public class RatingService
    {
        private readonly ICustomerRepository customerRepository;
        private readonly IAvailableTaxiRepository availableTaxiRepository;

        public RatingService(ICustomerRepository customerRepository, IAvailableTaxiRepository availableTaxiRepository)
        {
            this.customerRepository = customerRepository;
            this.availableTaxiRepository = availableTaxiRepository;
        }

        public void RateCustomer(Taxi taxi, Customer customer, int rate)
        {
            customer.Rate(rate);
            customerRepository.Update(customer);
        }

        public void RateTaxi(Customer customer, Taxi taxi, int rate)
        {
            taxi.Rate(rate);
            availableTaxiRepository.Update(taxi);
        }
    }
}