namespace TakeMeThere.Services
{
    public class RatingCalculator
    {
        public double CalculateNewAverage(double? currentRating, int rate, int timesRated)
        {
            return (((currentRating ?? 0) + rate) / timesRated);
        }
    }
}