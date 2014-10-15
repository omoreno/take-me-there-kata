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
                            .Where(x => MeetsCustomerNeeds(x.Features, searchCriteria.CustomerNeeds))
                            .Where(x => MeetsCustomerPreferences(x, searchCriteria.Customer));

            if (searchCriteria.Filter == TaxiSearchFilter.MostAffordable)
                return taxis.OrderBy(x => x.Features.Price).Take(MaxResults).ToList();

            return taxis.OrderBy(x => x.DistanceToCustomer(searchCriteria.CustomerLocation)).Take(MaxResults).ToList();
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