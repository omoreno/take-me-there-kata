using System.Collections.Generic;
using System.Linq;
using TakeMeThere.Models;
using TakeMeThere.Repositories;

namespace TakeMeThere.Services
{
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
}