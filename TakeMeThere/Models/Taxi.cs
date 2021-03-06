﻿using System;
using TakeMeThere.Exceptions;
using TakeMeThere.Services;
using TakeMeThere.ValueObjects;

namespace TakeMeThere.Models
{
    public class Taxi
    {
        private readonly Location currentLocation;
        private readonly TaxiTripLength tripLength;
        public int WorkingLocationRadio { get; private set; }
        public int? MinimunCustomerRating { get; private set; }
        public string Id { get; private set; }
        public TaxiFeatures Features { get; private set; }
        public double? Rating { get; private set; }
        private int timesRated;
        private readonly RatingCalculator ratingCalculator = new RatingCalculator();
        private readonly RatingValidator ratingValidator = new RatingValidator();
        private readonly LocationService locationService = new LocationService();

        public bool NeedsCustomerWithMinimunRating { get { return MinimunCustomerRating.HasValue; } }

        public Taxi(TaxiFeatures taxiFeatures, Location currentLocation, TaxiAvailabilityPreferences taxiAvailabilityPreferences)
        {
            Id = Guid.NewGuid().ToString();
            Features = taxiFeatures;
            this.currentLocation = currentLocation;
            tripLength = taxiAvailabilityPreferences.TripLength;
            MinimunCustomerRating = taxiAvailabilityPreferences.MinimunCustomerRating;
            WorkingLocationRadio = taxiAvailabilityPreferences.WorkingLocationRadio;
        }

        public double GetDistanceToCustomer(Location customerLocation)
        {
            return locationService.GetDistanceInMetersBetween(currentLocation, customerLocation);
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
            var otherTaxi = obj as Taxi;
            if (otherTaxi == null)
                return false;

            return Id.Equals(otherTaxi.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}