using System;

namespace TakeMeThere.Models
{
    public class AvailableTaxi
    {
        private readonly Location currentLocation;
        private readonly TaxiTripLength tripLength;
        private readonly int minimunCustomerRating;
        private readonly int workingLocationRadio;
        public string Id { get; private set; }
        public TaxiFeatures Features { get; private set; }

        public AvailableTaxi(TaxiFeatures taxiFeatures, Location currentLocation, TaxiAvailabilityPreferences taxiAvailabilityPreferences)
        {
            Id = new Guid().ToString();
            Features = taxiFeatures;
            this.currentLocation = currentLocation;
            tripLength = taxiAvailabilityPreferences.TripLength;
            minimunCustomerRating = taxiAvailabilityPreferences.MinimunCustomerRating;
            workingLocationRadio = taxiAvailabilityPreferences.WorkingLocationRadio;
        }

        public double DistanceToCustomer(Location customerLocation)
        {
            return Math.Sqrt(Math.Pow((currentLocation.Latitude - customerLocation.Latitude), 2) +
                  Math.Pow((currentLocation.Longitude - customerLocation.Longitude), 2));
        }
    }
}