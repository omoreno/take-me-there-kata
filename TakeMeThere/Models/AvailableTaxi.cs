﻿using System;

namespace TakeMeThere.Models
{
    public class AvailableTaxi
    {
        private readonly Location currentLocation;
        public TaxiOwnerPreferences TaxiOwnerPreferences { get; private set; }
        private readonly TaxiTripLength tripLength;
        private readonly int minimunCustomerRating;
        private readonly int workingLocationRadio;
        public string Id { get; private set; }
        public TaxiFeatures Features { get; private set; }
        public double? Rating { get; private set; }

        public bool NeedsCustomerWithMinimunRating { get { return TaxiOwnerPreferences.CustomerMinimunRating.HasValue; } }

        public AvailableTaxi(TaxiFeatures taxiFeatures, Location currentLocation, TaxiAvailabilityPreferences taxiAvailabilityPreferences, TaxiOwnerPreferences taxiOwnerPreferences)
        {
            Id = new Guid().ToString();
            Features = taxiFeatures;
            this.currentLocation = currentLocation;
            TaxiOwnerPreferences = taxiOwnerPreferences;
            tripLength = taxiAvailabilityPreferences.TripLength;
            minimunCustomerRating = taxiAvailabilityPreferences.MinimunCustomerRating;
            workingLocationRadio = taxiAvailabilityPreferences.WorkingLocationRadio;
        }

        public double DistanceToCustomer(Location customerLocation)
        {
            return Math.Sqrt(Math.Pow((currentLocation.Latitude - customerLocation.Latitude), 2) +
                  Math.Pow((currentLocation.Longitude - customerLocation.Longitude), 2));
        }
    }
}