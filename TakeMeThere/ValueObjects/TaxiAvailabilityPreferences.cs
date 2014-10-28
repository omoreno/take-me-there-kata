namespace TakeMeThere.ValueObjects
{
    public class TaxiAvailabilityPreferences
    {
        public TaxiTripLength TripLength { get; private set; }
        public int? MinimunCustomerRating { get; private set; }
        public int WorkingLocationRadio { get; private set; }

        public TaxiAvailabilityPreferences(TaxiTripLength tripLength, int? minimunCustomerRating, int workingLocationRadio)
        {
            TripLength = tripLength;
            MinimunCustomerRating = minimunCustomerRating;
            WorkingLocationRadio = workingLocationRadio;
        }
    }
}