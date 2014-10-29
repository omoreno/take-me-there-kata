using System;
using TakeMeThere.Exceptions;
using TakeMeThere.Models;
using TakeMeThere.Repositories;

namespace TakeMeThere.Services
{
    public class BookingService
    {
        private readonly ITaxiRepository taxiRepository;
        private readonly IBookingRepository bookingRepository;
        public const int CancellationTimeInMinutes = 10;

        public BookingService(ITaxiRepository taxiRepository, IBookingRepository bookingRepository)
        {
            this.taxiRepository = taxiRepository;
            this.bookingRepository = bookingRepository;
        }

        public string BookTaxi(BookingRequest bookingRequest)
        {
            if (!taxiRepository.Exists(bookingRequest.TaxiId))
                throw new AlreadyBookedTaxi();

            var booking = new Booking(bookingRequest.CustomerId, bookingRequest.TaxiId, DateTime.Now);
            bookingRepository.Save(booking);
            taxiRepository.Delete(bookingRequest.TaxiId);
            return booking.Reference;
        }

        public void CancelBooking(string bookreference)
        {
            var booking = bookingRepository.FindByReference(bookreference);
            if (CancellationTimeWasReached(booking.BookingDate))
                throw new CancellationTimeLimitReached();
            
            bookingRepository.Delete(booking);
        }

        private static bool CancellationTimeWasReached(DateTime bookingDate)
        {
            return bookingDate.AddMinutes(CancellationTimeInMinutes) < DateTime.Now;
        }
    }
}