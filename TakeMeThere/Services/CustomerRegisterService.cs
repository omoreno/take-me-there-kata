using TakeMeThere.Models;
using TakeMeThere.Repositories;

namespace TakeMeThere.Services
{
    public class CustomerRegisterService
    {
        private readonly ICustomerRepository customerRepository;

        public CustomerRegisterService(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }

        public void RegisterCustomer(Customer customer)
        {
            customerRepository.Save(customer);
        }
    }
}