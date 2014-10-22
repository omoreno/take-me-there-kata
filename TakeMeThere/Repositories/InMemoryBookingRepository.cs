using System.Collections.Generic;
using System.Linq;
using TakeMeThere.Models;

namespace TakeMeThere.Repositories
{
    public class InMemoryBookingRepository : IBookingRepository
    {
        private readonly List<Booking> bookings;

        public InMemoryBookingRepository()
        {
            if (bookings == null)
                bookings = new List<Booking>();
        }

        public void Save(Booking booking)
        {
            bookings.Add(booking);
        }

        public bool Exists(string bookReference)
        {
            return bookings.Any(x => x.Reference == bookReference);
        }
    }
}