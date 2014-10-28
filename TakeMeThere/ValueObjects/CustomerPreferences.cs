namespace TakeMeThere.ValueObjects
{
    public class CustomerPreferences
    {
        public int? TaxiMinimunRating { get; private set; }

        public CustomerPreferences(int? taxiMinimunRating)
        {
            TaxiMinimunRating = taxiMinimunRating;
        }
    }
}