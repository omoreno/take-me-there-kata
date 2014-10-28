using System;
using TakeMeThere.Models;
using TakeMeThere.ValueObjects;

namespace TakeMeThere.Services
{
    public class LocationService
    {
        public double GetDistanceBetween(Location source, Location destination)
        {
            return Math.Sqrt(Math.Pow((source.Latitude - destination.Latitude), 2) +
                             Math.Pow((source.Longitude - destination.Longitude), 2));
        }
    }
}