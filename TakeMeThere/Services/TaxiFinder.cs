using System.Collections.Generic;
using System.Linq;
using TakeMeThere.Models;
using TakeMeThere.Repositories;

namespace TakeMeThere.Services
{
    public class TaxiFinder
    {
        private readonly IAvailableTaxiRepository availableTaxiRepository;
        private const int MaxResults = 10;

        public TaxiFinder(IAvailableTaxiRepository availableTaxiRepository)
        {
            this.availableTaxiRepository = availableTaxiRepository;
        }

        public List<AvailableTaxi> GetTaxis(TaxiSearchCriteria searchCriteria)
        {
            var taxis = availableTaxiRepository
                            .GetAll()
                            .Where(taxi => MeetsCustomerNeeds(taxi.Features, searchCriteria.CustomerNeeds))
                            .Where(taxi => MeetsCustomerPreferences(taxi, searchCriteria.Customer))
                            .Where(taxi => MeetsTaxiOwnerPreferences(taxi, searchCriteria.Customer, searchCriteria.CustomerLocation));

            if (searchCriteria.Filter == TaxiSearchFilter.MostAffordable)
                return taxis.OrderBy(taxi => taxi.Features.Price).Take(MaxResults).ToList();

            return taxis.OrderBy(taxi => taxi.DistanceToCustomer(searchCriteria.CustomerLocation)).Take(MaxResults).ToList();
        }

        private bool MeetsTaxiOwnerPreferences(AvailableTaxi taxi, Customer customer, Location customerLocation)
        {
            if (taxi.NeedsCustomerWithMinimunRating)
                return (customer.Rating >= taxi.MinimunCustomerRating) && taxi.DistanceToCustomer(customerLocation) <= taxi.WorkingLocationRadio;
            return taxi.DistanceToCustomer(customerLocation) <= taxi.WorkingLocationRadio;
        }

        private bool MeetsCustomerPreferences(AvailableTaxi taxi, Customer customer)
        {
            if (customer.NeedsTaxiWithMinimunRating)
                return taxi.Rating.HasValue && taxi.Rating.Value >= customer.Preferences.TaxiMinimunRating;

            return true;
        }

        private bool MeetsCustomerNeeds(TaxiFeatures taxiFeatures, CustomerNeeds customerNeeds)
        {
            return (customerNeeds == null) ||
                    (taxiFeatures.Size == customerNeeds.Size &&
                       taxiFeatures.NumberOfSeats == customerNeeds.NumberOfSeats &&
                       taxiFeatures.AirConditioned == customerNeeds.WithAirConditioned &&
                       taxiFeatures.WheelchairAccesible == customerNeeds.WheelchairAccessible &&
                       taxiFeatures.ExtraBaggageSpace == customerNeeds.WithExtraBagaggeSpace &&
                       taxiFeatures.LuxuriousEquipment == customerNeeds.WithLuxuriousEquipment);
        }
    }
}