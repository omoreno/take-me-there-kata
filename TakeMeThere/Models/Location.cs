namespace TakeMeThere.Models
{
    public class Location
    {
        private double longitude;
        private double latitude;

        public Location(double latitude, double longitude)
        {
            this.latitude = latitude;
            this.longitude = longitude;
        }
    }
}