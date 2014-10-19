using Moq;
using NUnit.Framework;
using TakeMeThere.Models;
using TakeMeThere.Repositories;
using TakeMeThere.Services;

namespace TakeMeThere
{
    [TestFixture]
    public class CustomerRegistrationTests
    {
        [Test]
        public void ShouldRegisterNewCustomer()
        {
            var customerRepository = new Mock<ICustomerRepository>();
            var registerService = new CustomerRegisterService(customerRepository.Object);
            var api = new CommandLineInterface(null, null, null, registerService);

            api.RegisterCustomer(new Customer(new CustomerPreferences(null)));

            customerRepository.Verify(x => x.Save(It.IsAny<Customer>()));
        }
    }
}
