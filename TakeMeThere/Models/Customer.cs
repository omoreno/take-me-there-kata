using System;

namespace TakeMeThere.Models
{
    public class Customer
    {
        public string Id { get; private set; }
        public CustomerPreferences Preferences { get; private set; }
        public bool NeedsTaxiWithMinimunRating { get { return Preferences.TaxiMinimunRating.HasValue; } }
        public double? Rating { get; private set; }

        public Customer(CustomerPreferences preferences)
        {
            Id = new Guid().ToString();
            Preferences = preferences;
        }
    }
}