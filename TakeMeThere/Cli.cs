using System.Collections.Generic;
using TakeMeThere.Models;
using TakeMeThere.Services;

namespace TakeMeThere
{
    public class CommandLineInterface
    {
        private readonly BookingService bookingService;
        private readonly TaxiFinder taxiFinder;
        private readonly TaxiRegisterService registerService;

        public CommandLineInterface(BookingService bookingService, TaxiRegisterService registerService, TaxiFinder taxiFinder)
        {
            this.bookingService = bookingService;
            this.registerService = registerService;
            this.taxiFinder = taxiFinder;
        }

        public void RegisterTaxi(TaxiFeatures taxiFeatures, Location currentLocation, TaxiAvailabilityPreferences taxiAvailabilityPreferences)
        {
            var availableTaxi = new AvailableTaxi(taxiFeatures, currentLocation, taxiAvailabilityPreferences, new TaxiOwnerPreferences(null));
            registerService.RegisterTaxi(availableTaxi);
        }

        public string BookTaxi(AvailableTaxi availableTaxi, Customer customer, Location startLocation, Location endLocation, double price)
        {
            var bookingRequest = new BookingRequest(availableTaxi.Id, customer.Id, startLocation, endLocation, price);
            return bookingService.BookTaxi(bookingRequest);
        }

        public List<AvailableTaxi> GetTaxis(Customer customer, Location customerLocation, TaxiSearchFilter filter, CustomerNeeds customerNeeds)
        {
            var criteria = new TaxiSearchCriteria(customer, customerLocation, customerNeeds, filter);
            return taxiFinder.GetTaxis(criteria);
        }
    }
}