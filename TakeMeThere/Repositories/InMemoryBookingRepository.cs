using System.Collections.Generic;
using System.Linq;
using TakeMeThere.Exceptions;
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

        public Booking FindByReference(string bookReference)
        {
            var booking = bookings.FirstOrDefault(x => x.Reference == bookReference);
            if (booking == null)
                throw new BookReferenceNotExists();
            
            return booking;
        }
    }
}