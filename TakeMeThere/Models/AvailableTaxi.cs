namespace TakeMeThere.Models
{
    public class AvailableTaxi
    {
        private readonly Taxi _taxi;
        private readonly Location _currentLocation;
        private readonly TaxiTripLength tripLength;
        private readonly int minimunCustomerRating;
        private readonly int workingLocationRadio;

        public AvailableTaxi(Taxi taxi, Location currentLocation, TaxiAvailabilityPreferences taxiAvailabilityPreferences)
        {
            _taxi = taxi;
            _currentLocation = currentLocation;
            tripLength = taxiAvailabilityPreferences.TripLength;
            minimunCustomerRating = taxiAvailabilityPreferences.MinimunCustomerRating;
            workingLocationRadio = taxiAvailabilityPreferences.WorkingLocationRadio;
        }
    }
}