using System;

namespace TakeMeThere.Models
{
    public class Booking
    {
        public string CustomerId { get; private set; }
        public string TaxiId { get; private set; }
        public string Reference { get; private set; }
        public DateTime BookingDate { get; private set; }

        public Booking(string customerId, string taxiId, DateTime bookingDate)
        {
            Reference = Guid.NewGuid().ToString();
            CustomerId = customerId;
            TaxiId = taxiId;
            BookingDate = bookingDate;
        }
    }
}