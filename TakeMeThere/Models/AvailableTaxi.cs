namespace TakeMeThere.Models
{
    public class AvailableTaxi
    {
        private readonly Taxi taxi;
        private readonly Location currentLocation;
        private readonly TaxiTripLength tripLength;
        private readonly int minimunCustomerRating;
        private readonly int workingLocationRadio;
        public int Price { get; private set; }

        public AvailableTaxi(Taxi taxi, Location currentLocation, TaxiAvailabilityPreferences taxiAvailabilityPreferences)
        {
            this.taxi = taxi;
            this.currentLocation = currentLocation;
            tripLength = taxiAvailabilityPreferences.TripLength;
            minimunCustomerRating = taxiAvailabilityPreferences.MinimunCustomerRating;
            workingLocationRadio = taxiAvailabilityPreferences.WorkingLocationRadio;
            Price = taxi.Price;
        }
    }
}