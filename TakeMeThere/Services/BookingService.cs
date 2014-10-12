using TakeMeThere.Exceptions;
using TakeMeThere.Models;
using TakeMeThere.Repositories;

namespace TakeMeThere.Services
{
    public class BookingService
    {
        private readonly IAvailableTaxiRepository availableTaxiRepository;
        private readonly IBookingRepository bookingRepository;

        public BookingService(IAvailableTaxiRepository availableTaxiRepository, IBookingRepository bookingRepository)
        {
            this.availableTaxiRepository = availableTaxiRepository;
            this.bookingRepository = bookingRepository;
        }

        public string BookTaxi(BookingRequest bookingRequest)
        {
            if (!availableTaxiRepository.Exists(bookingRequest.TaxiId))
                throw new AlreadyBookedTaxi();

            bookingRepository.Save(bookingRequest);
            availableTaxiRepository.Delete(bookingRequest.TaxiId);
            return bookingRequest.Id;
        }
    }
}