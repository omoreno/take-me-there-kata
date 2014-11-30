using System.Collections.Generic;
using System.Linq;
using TakeMeThere.Models;
using TakeMeThere.Repositories;
using TakeMeThere.ValueObjects;

namespace TakeMeThere.Services
{
    public class TaxiFinder
    {
        private readonly ITaxiRepository taxiRepository;
        private const int MaxResults = 10;

        public TaxiFinder(ITaxiRepository taxiRepository)
        {
            this.taxiRepository = taxiRepository;
        }

        public List<Taxi> GetTaxis(TaxiSearchCriteria searchCriteria)
        {
            var taxis = taxiRepository
                            .GetAll()
                            .Where(taxi => MeetsCustomerNeeds(taxi.Features, searchCriteria.CustomerNeeds))
                            .Where(taxi => MeetsCustomerPreferences(taxi, searchCriteria.Customer))
                            .Where(taxi => MeetsTaxiOwnerPreferences(taxi, searchCriteria.Customer, searchCriteria.CustomerLocation));

            if (searchCriteria.Filter == TaxiSearchFilter.MostAffordable)
                return taxis.OrderBy(taxi => taxi.Features.Price).Take(MaxResults).ToList();
            
            if (searchCriteria.Filter == TaxiSearchFilter.BestRated)
                return taxis.OrderByDescending(taxi => taxi.Rating).Take(MaxResults).ToList();
            
            return taxis.OrderBy(taxi => taxi.GetDistanceToCustomer(searchCriteria.CustomerLocation)).Take(MaxResults).ToList();
        }

        private bool MeetsTaxiOwnerPreferences(Taxi taxi, Customer customer, Location customerLocation)
        {
            if (taxi.NeedsCustomerWithMinimunRating)
                return (customer.Rating >= taxi.MinimunCustomerRating) && taxi.GetDistanceToCustomer(customerLocation) <= taxi.WorkingLocationRadio;
            return taxi.GetDistanceToCustomer(customerLocation) <= taxi.WorkingLocationRadio;
        }

        private bool MeetsCustomerPreferences(Taxi taxi, Customer customer)
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