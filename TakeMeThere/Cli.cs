using System.Collections.Generic;
using TakeMeThere.Models;
using TakeMeThere.Services;

namespace TakeMeThere
{
    public class CommandLineInterface
    {
        private readonly BookingService bookingService;
        private readonly TaxiFinder taxiFinder;
        private readonly CustomerRegisterService customerRegisterService;
        private readonly TaxiRegisterService registerService;

        public CommandLineInterface(BookingService bookingService, TaxiRegisterService registerService, TaxiFinder taxiFinder, CustomerRegisterService customerRegisterService)
        {
            this.bookingService = bookingService;
            this.registerService = registerService;
            this.taxiFinder = taxiFinder;
            this.customerRegisterService = customerRegisterService;
        }

        public void RegisterTaxi(TaxiFeatures taxiFeatures, Location currentLocation, TaxiAvailabilityPreferences taxiAvailabilityPreferences)
        {
            var availableTaxi = new Taxi(taxiFeatures, currentLocation, taxiAvailabilityPreferences);
            registerService.RegisterTaxi(availableTaxi);
        }

        public string BookTaxi(Taxi taxi, Customer customer, Location startLocation, Location endLocation, double price)
        {
            var bookingRequest = new BookingRequest(taxi.Id, customer.Id, startLocation, endLocation, price);
            return bookingService.BookTaxi(bookingRequest);
        }

        public List<Taxi> GetTaxis(Customer customer, Location customerLocation, TaxiSearchFilter filter, CustomerNeeds customerNeeds)
        {
            var criteria = new TaxiSearchCriteria(customer, customerLocation, customerNeeds, filter);
            return taxiFinder.GetTaxis(criteria);
        }

        public void RegisterCustomer(Customer customer)
        {
            customerRegisterService.RegisterCustomer(customer);
        }

        public void CancelBooking(string bookreference)
        {
            bookingService.CancelBooking(bookreference);
        }
    }
}