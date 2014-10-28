using System;
using TakeMeThere.ValueObjects;

namespace TakeMeThere.Models
{
    public class BookingRequest
    {
        public string TaxiId { get; private set; }
        public string CustomerId { get; private set; }
        private readonly Location startLocation;
        private readonly Location endLocation;
        private readonly double price;

        public BookingRequest(string taxiId, string customerId, Location startLocation, Location endLocation, double price)
        {
            TaxiId = taxiId;
            CustomerId = customerId;
            this.startLocation = startLocation;
            this.endLocation = endLocation;
            this.price = price;
        }
    }
}