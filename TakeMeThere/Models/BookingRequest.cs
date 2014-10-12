using System;

namespace TakeMeThere.Models
{
    public class BookingRequest
    {
        public string Id { get; private set;  }
        private readonly string taxiId;
        private readonly string customerId;

        public BookingRequest(string taxiId, string customerId)
        {
            Id = new Guid().ToString();
            this.taxiId = taxiId;
            this.customerId = customerId;
        }
    }
}