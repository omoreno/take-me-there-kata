using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TakeMeThere.Exceptions;
using TakeMeThere.Models;
using TakeMeThere.Repositories;

namespace TakeMeThere
{
    class Program
    {
        static void Main(string[] args)
        {
            throw new NotImplementedException();
        }
    }

    public class Api
    {
        private readonly IAvailableTaxiRepository availableTaxiRepository;
        private readonly IBookingRepository bookingRepository;

        public Api(IAvailableTaxiRepository availableTaxiRepository, IBookingRepository bookingRepository)
        {
            this.availableTaxiRepository = availableTaxiRepository;
            this.bookingRepository = bookingRepository;
        }

        public void RegisterTaxi(Taxi taxi, Location currentLocation, TaxiAvailabilityPreferences taxiAvailabilityPreferences)
        {
            var availableTaxi = new AvailableTaxi(taxi, currentLocation, taxiAvailabilityPreferences);
            availableTaxiRepository.Save(availableTaxi);
        }

        public string BookTaxi(Taxi taxi, Customer customer, Location startLocation, Location endLocation, double price)
        {
            if (!availableTaxiRepository.Exists(taxi.Id))
                throw new AlreadyBookedTaxi();

            var bookingRequest = new BookingRequest(taxi.Id, customer.Id);
            bookingRepository.Save(bookingRequest);
            availableTaxiRepository.Delete(taxi.Id);
            return bookingRequest.Id;
        }

        public List<AvailableTaxi> GetTaxis(Customer customer, Location customerLocation, TaxiSearchFilter filter, CustomerNeeds customerNeeds)
        {
            var taxis = availableTaxiRepository.GetAll();
            if (filter == TaxiSearchFilter.MostAffordable)
                return taxis.OrderBy(x => x.Price).Take(10).ToList();
            return taxis.OrderBy(x => x.DistanceToCustomer(customerLocation)).Take(10).ToList();
        }
    }
}
