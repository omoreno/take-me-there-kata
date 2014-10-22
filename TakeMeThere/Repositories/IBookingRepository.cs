using TakeMeThere.Models;

namespace TakeMeThere.Repositories
{
    public interface IBookingRepository
    {
        void Save(Booking bookingRequest);
        bool Exists(string bookReference);
        Booking FindByReference(string bookReference);
    }
}