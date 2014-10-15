namespace TakeMeThere.Models
{
    public class TaxiOwnerPreferences
    {
        public int? CustomerMinimunRating { get; private set; }

        public TaxiOwnerPreferences(int? customerMinimunRating)
        {
            CustomerMinimunRating = customerMinimunRating;
        }
    }
}