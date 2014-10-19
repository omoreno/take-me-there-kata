using System;
using TakeMeThere.Exceptions;

namespace TakeMeThere.Models
{
    public class Customer
    {
        public string Id { get; private set; }
        public CustomerPreferences Preferences { get; private set; }
        public bool NeedsTaxiWithMinimunRating { get { return Preferences.TaxiMinimunRating.HasValue; } }
        public double? Rating { get; private set; }
        private int timesRated;

        public Customer(CustomerPreferences preferences)
        {
            Id = new Guid().ToString();
            Preferences = preferences;
        }

        public void Rate(int rate)
        {
            if (rate < 1 || rate > 5)
                throw new NotValidRating();
            timesRated++;
            Rating = (((Rating ?? 0) + rate) / timesRated);
        }
    }
}