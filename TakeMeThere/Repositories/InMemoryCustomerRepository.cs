using System.Collections.Generic;
using TakeMeThere.Models;

namespace TakeMeThere.Repositories
{
    public class InMemoryCustomerRepository : ICustomerRepository
    {
        private readonly List<Customer> customers; 
        public InMemoryCustomerRepository()
        {
            if (customers == null)
                customers = new List<Customer>();
        }

        public void Update(Customer customer)
        {
            customers.Remove(customer);
            customers.Add(customer);
        }

        public void Save(Customer customer)
        {
            customers.Add(customer);
        }
    }
}