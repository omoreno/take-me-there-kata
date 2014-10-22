using TakeMeThere.Models;
using TakeMeThere.Repositories;

namespace TakeMeThere.Services
{
    public class RatingService
    {
        private readonly ICustomerRepository customerRepository;
        private readonly ITaxiRepository taxiRepository;

        public RatingService(ICustomerRepository customerRepository, ITaxiRepository taxiRepository)
        {
            this.customerRepository = customerRepository;
            this.taxiRepository = taxiRepository;
        }

        public void RateCustomer(Taxi taxi, Customer customer, int rate)
        {
            customer.Rate(rate);
            customerRepository.Update(customer);
        }

        public void RateTaxi(Customer customer, Taxi taxi, int rate)
        {
            taxi.Rate(rate);
            taxiRepository.Update(taxi);
        }
    }
}