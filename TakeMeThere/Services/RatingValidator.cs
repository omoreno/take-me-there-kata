namespace TakeMeThere.Services
{
    public class RatingValidator
    {
        public bool IsValid(int rate)
        {
            return rate >= 1 && rate <= 5;
        }
    }
}