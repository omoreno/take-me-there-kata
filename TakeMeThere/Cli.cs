using System.Collections.Generic;
using System.Linq;
using TakeMeThere.Exceptions;
using TakeMeThere.Models;
using TakeMeThere.Repositories;

namespace TakeMeThere
{
    public class BookingService
    {
        private readonly IAvailableTaxiRepository availableTaxiRepository;
        private readonly IBookingRepository bookingRepository;

        public BookingService(IAvailableTaxiRepository availableTaxiRepository, IBookingRepository bookingRepository)
        {
            this.availableTaxiRepository = availableTaxiRepository;
            this.bookingRepository = bookingRepository;
        }

        public string BookTaxi(BookingRequest bookingRequest)
        {
            if (!availableTaxiRepository.Exists(bookingRequest.TaxiId))
                throw new AlreadyBookedTaxi();

            bookingRepository.Save(bookingRequest);
            availableTaxiRepository.Delete(bookingRequest.TaxiId);
            return bookingRequest.Id;
        }
    }

    public class TaxiFinder
    {
        private readonly IAvailableTaxiRepository availableTaxiRepository;

        public TaxiFinder(IAvailableTaxiRepository availableTaxiRepository)
        {
            this.availableTaxiRepository = availableTaxiRepository;
        }

        public List<AvailableTaxi> GetTaxis(TaxiSearchCriteria searchCriteria)
        {
            var taxis = availableTaxiRepository.GetAll()
                                               .Where(x => MeetsCustomerNeeds(x.Features, searchCriteria.CustomerNeeds));

            if (searchCriteria.Filter == TaxiSearchFilter.MostAffordable)
                return taxis.OrderBy(x => x.Features.Price).Take(10).ToList();

            return taxis.OrderBy(x => x.DistanceToCustomer(searchCriteria.CustomerLocation)).Take(10).ToList();
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

    public class TaxiRegisterService
    {
        private readonly IAvailableTaxiRepository availableTaxiRepository;

        public TaxiRegisterService(IAvailableTaxiRepository availableTaxiRepository)
        {
            this.availableTaxiRepository = availableTaxiRepository;
        }

        public void RegisterTaxi(AvailableTaxi availableTaxi)
        {
            availableTaxiRepository.Save(availableTaxi);
        }
    }

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
            var availableTaxi = new AvailableTaxi(taxiFeatures, currentLocation, taxiAvailabilityPreferences);
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

    public class TaxiSearchCriteria
    {
        public Customer Customer { get; private set; }
        public Location CustomerLocation { get; private set; }
        public CustomerNeeds CustomerNeeds { get; private set; }
        public TaxiSearchFilter Filter { get; private set; }

        public TaxiSearchCriteria(Customer customer, Location customerLocation, CustomerNeeds customerNeeds, TaxiSearchFilter filter)
        {
            Customer = customer;
            CustomerLocation = customerLocation;
            CustomerNeeds = customerNeeds;
            Filter = filter;
        }
    }
}