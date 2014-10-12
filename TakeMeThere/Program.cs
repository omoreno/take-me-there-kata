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

        public void RegisterTaxi(TaxiFeatures taxiFeatures, Location currentLocation, TaxiAvailabilityPreferences taxiAvailabilityPreferences)
        {
            var availableTaxi = new AvailableTaxi(taxiFeatures, currentLocation, taxiAvailabilityPreferences);
            availableTaxiRepository.Save(availableTaxi);
        }

        public string BookTaxi(AvailableTaxi availableTaxi, Customer customer, Location startLocation, Location endLocation, double price)
        {
            if (!availableTaxiRepository.Exists(availableTaxi.Id))
                throw new AlreadyBookedTaxi();

            var bookingRequest = new BookingRequest(availableTaxi.Id, customer.Id);
            bookingRepository.Save(bookingRequest);
            availableTaxiRepository.Delete(availableTaxi.Id);
            return bookingRequest.Id;
        }

        public List<AvailableTaxi> GetTaxis(Customer customer, Location customerLocation, TaxiSearchFilter filter, CustomerNeeds customerNeeds)
        {
            var taxis = availableTaxiRepository.GetAll()
                        .Where(x => MeetsCustomerNeeds(x.Features, customerNeeds));
            
            if (filter == TaxiSearchFilter.MostAffordable)
                return taxis.OrderBy(x => x.Features.Price).Take(10).ToList();
            
            return taxis.OrderBy(x => x.DistanceToCustomer(customerLocation)).Take(10).ToList();
        }

        private bool MeetsCustomerNeeds(TaxiFeatures taxiFeatures, CustomerNeeds customerNeeds)
        {
            if (customerNeeds == null)
                return true;
            return taxiFeatures.Size == customerNeeds.Size &&
                    taxiFeatures.NumberOfSeats == customerNeeds.NumberOfSeats &&
                    taxiFeatures.AirConditioned == customerNeeds.WithAirConditioned &&
                    taxiFeatures.WheelchairAccesible == customerNeeds.WheelchairAccessible &&
                    taxiFeatures.ExtraBaggageSpace == customerNeeds.WithExtraBagaggeSpace &&
                    taxiFeatures.LuxuriousEquipment == customerNeeds.WithLuxuriousEquipment;
        }
    }
}
