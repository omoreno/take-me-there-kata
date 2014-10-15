namespace TakeMeThere.Models
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