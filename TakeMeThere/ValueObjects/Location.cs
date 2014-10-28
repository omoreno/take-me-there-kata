namespace TakeMeThere.ValueObjects
{
    public class Location
    {
        public double Longitude { get; private set; }
        public double Latitude { get; private set; }

        public Location(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}