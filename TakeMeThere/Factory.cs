using TakeMeThere.Repositories;
using TakeMeThere.Services;

namespace TakeMeThere
{
    public class Factory
    {
        public static CommandLineInterface CommandLineInterface()
        {
            return new CommandLineInterface(BookingService(), TaxiRegisterService(), TaxiFinder());
        }

        public static BookingService BookingService()
        {
            return new BookingService(new InMemoryAvailableTaxiRepository(), new InMemoryBookingRepository());
        }

        public static TaxiRegisterService TaxiRegisterService()
        {
            return new TaxiRegisterService(new InMemoryAvailableTaxiRepository());
        }

        public static TaxiFinder TaxiFinder()
        {
            return new TaxiFinder(new InMemoryAvailableTaxiRepository());
        }

        public static RatingService RatingService()
        {
            return new RatingService(new InMemoryCustomerRepository(), new InMemoryAvailableTaxiRepository());
        }
    }
}