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

        public BookingService(ITaxiRepository taxiRepository, IBookingRepository bookingRepository)
        {
            this.taxiRepository = taxiRepository;
            this.bookingRepository = bookingRepository;
        }

        public string BookTaxi(BookingRequest bookingRequest)
        {
            if (!taxiRepository.Exists(bookingRequest.TaxiId))
                throw new AlreadyBookedTaxi();

            bookingRepository.Save(new Booking(bookingRequest.CustomerId, bookingRequest.TaxiId, DateTime.Now));
            taxiRepository.Delete(bookingRequest.TaxiId);
            return bookingRequest.Id;
        }

        public void CancelBooking(string bookreference)
        {
            if (!bookingRepository.Exists(bookreference))
                throw new BookReferenceNotExists();
            
            var booking = bookingRepository.FindByReference(bookreference);
            throw new CancellationTimeLimitReached();
        }
    }
}