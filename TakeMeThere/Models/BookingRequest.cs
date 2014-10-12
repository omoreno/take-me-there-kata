using System;

namespace TakeMeThere.Models
{
    public class BookingRequest
    {
        public string Id { get; private set;  }
        public string TaxiId { get; private set; }
        public string CustomerId { get; private set; }
        private readonly Location startLocation;
        private readonly Location endLocation;
        private readonly double price;

        public BookingRequest(string taxiId, string customerId, Location startLocation, Location endLocation, double price)
        {
            Id = new Guid().ToString();
            TaxiId = taxiId;
            CustomerId = customerId;
            this.startLocation = startLocation;
            this.endLocation = endLocation;
            this.price = price;
        }
    }
}