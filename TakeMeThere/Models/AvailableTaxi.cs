using System;

namespace TakeMeThere.Models
{
    public class AvailableTaxi
    {
        private readonly Location currentLocation;
        private readonly TaxiTripLength tripLength;
        public int WorkingLocationRadio { get; private set; }
        public int? MinimunCustomerRating { get; private set; }
        public string Id { get; private set; }
        public TaxiFeatures Features { get; private set; }
        public double? Rating { get; private set; }

        public bool NeedsCustomerWithMinimunRating { get { return MinimunCustomerRating.HasValue; } }

        public AvailableTaxi(TaxiFeatures taxiFeatures, Location currentLocation, TaxiAvailabilityPreferences taxiAvailabilityPreferences)
        {
            Id = new Guid().ToString();
            Features = taxiFeatures;
            this.currentLocation = currentLocation;
            tripLength = taxiAvailabilityPreferences.TripLength;
            MinimunCustomerRating = taxiAvailabilityPreferences.MinimunCustomerRating;
            WorkingLocationRadio = taxiAvailabilityPreferences.WorkingLocationRadio;
        }

        public double DistanceToCustomer(Location customerLocation)
        {
            return Math.Sqrt(Math.Pow((currentLocation.Latitude - customerLocation.Latitude), 2) +
                  Math.Pow((currentLocation.Longitude - customerLocation.Longitude), 2));
        }
    }
}