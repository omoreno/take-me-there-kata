using TakeMeThere.Models;

namespace TakeMeThere.Repositories
{
    public interface IBookingRepository
    {
        void Save(Booking bookingRequest);
    }
}