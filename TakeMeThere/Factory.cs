using TakeMeThere.Repositories;
using TakeMeThere.Services;

namespace TakeMeThere
{
    public class Factory
    {
        private static InMemoryCustomerRepository inMemoryCustomerRepository;
        private static InMemoryAvailableTaxiRepository inMemoryAvailableTaxiRepository;
        private static InMemoryBookingRepository inMemoryBookingRepository;

        public static CommandLineInterface CommandLineInterface()
        {
            return new CommandLineInterface(BookingService(), TaxiRegisterService(), TaxiFinder(), CustomerRegisterService());
        }

        public static InMemoryCustomerRepository InMemoryCustomerRepository()
        {
            if (inMemoryCustomerRepository == null)
                inMemoryCustomerRepository = new InMemoryCustomerRepository();
            return inMemoryCustomerRepository;
        }

        public static InMemoryAvailableTaxiRepository InMemoryAvailableTaxiRepository()
        {
            if (inMemoryAvailableTaxiRepository == null)
                inMemoryAvailableTaxiRepository = new InMemoryAvailableTaxiRepository();
            return inMemoryAvailableTaxiRepository;
        }

        public static InMemoryBookingRepository InMemoryBookingRepository()
        {
            if (inMemoryBookingRepository == null)
                inMemoryBookingRepository = new InMemoryBookingRepository();
            return inMemoryBookingRepository;
        }

        public static BookingService BookingService()
        {
            return new BookingService(InMemoryAvailableTaxiRepository(), InMemoryBookingRepository());
        }

        public static CustomerRegisterService CustomerRegisterService()
        {
            return new CustomerRegisterService(InMemoryCustomerRepository());
        }

        public static TaxiRegisterService TaxiRegisterService()
        {
            return new TaxiRegisterService(InMemoryAvailableTaxiRepository());
        }

        public static TaxiFinder TaxiFinder()
        {
            return new TaxiFinder(InMemoryAvailableTaxiRepository());
        }

        public static RatingService RatingService()
        {
            return new RatingService(InMemoryCustomerRepository(), InMemoryAvailableTaxiRepository());
        }
    }
}