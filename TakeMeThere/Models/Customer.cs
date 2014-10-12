using System;

namespace TakeMeThere.Models
{
    public class Customer
    {
        public string Id { get; private set; }

        public Customer()
        {
            Id = new Guid().ToString();
        }
    }
}