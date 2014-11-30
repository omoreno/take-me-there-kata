namespace TakeMeThere.Services
{
    public class RatingValidator
    {
        private const int MinimunRate = 1;
        private const int MaximunRate = 5;

        public bool IsValid(int rate)
        {
            return rate >= MinimunRate && rate <= MaximunRate;
        }
    }
}