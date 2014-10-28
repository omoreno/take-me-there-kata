using System;
using TakeMeThere.Exceptions;
using TakeMeThere.Services;
using TakeMeThere.ValueObjects;

namespace TakeMeThere.Models
{
    public class Customer
    {
        public string Id { get; private set; }
        public CustomerPreferences Preferences { get; private set; }
        public bool NeedsTaxiWithMinimunRating { get { return Preferences.TaxiMinimunRating.HasValue; } }
        public double? Rating { get; private set; }
        private int timesRated;
        private readonly RatingCalculator ratingCalculator = new RatingCalculator();
        private readonly RatingValidator ratingValidator = new RatingValidator();

        public Customer(CustomerPreferences preferences)
        {
            Id = Guid.NewGuid().ToString();
            Preferences = preferences;
        }

        public void Rate(int rate)
        {
            if (!ratingValidator.IsValid(rate))
                throw new NotValidRating();
            timesRated++;
            Rating = ratingCalculator.CalculateNewAverage(Rating, rate, timesRated);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            var otherCustomer = obj as Customer;
            if (otherCustomer == null)
                return false;

            return Id.Equals(otherCustomer.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}