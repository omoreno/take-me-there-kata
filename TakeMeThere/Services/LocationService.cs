using System;
using TakeMeThere.ValueObjects;

namespace TakeMeThere.Services
{
    public class LocationService
    {
        public double GetDistanceInMetersBetween(Location source, Location destination)
        {
            //http://en.wikipedia.org/wiki/Haversine_formula
            const double earthRadioInKm = 6371; //Earths mean radio in km

            var sourceLatitudeSin = Math.Sin(Radians(source.Latitude));
            var destinationLatitudeSin = Math.Sin(Radians(destination.Latitude));
            var sourceLatitudeCos = Math.Cos(Radians(source.Latitude));
            var destinationLatitudeCos = Math.Cos(Radians(destination.Latitude));
            var longitudeDiferences = Math.Cos(Radians(source.Longitude) - Radians(destination.Longitude));

            return earthRadioInKm* Math.Acos(sourceLatitudeSin * destinationLatitudeSin + sourceLatitudeCos * destinationLatitudeCos * longitudeDiferences);
        }

        private double Radians(double number)
        {
            return number * Math.PI / 180;
        }

    }
}