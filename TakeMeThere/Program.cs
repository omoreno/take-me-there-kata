using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TakeMeThere.Exceptions;
using TakeMeThere.Models;
using TakeMeThere.Repositories;

namespace TakeMeThere
{
    class Program
    {
        static void Main(string[] args)
        {
            throw new NotImplementedException();
        }
    }

    public class Api
    {
        private readonly IAvailableTaxiRepository availableTaxiRepository;

        public Api(IAvailableTaxiRepository availableTaxiRepository)
        {
            this.availableTaxiRepository = availableTaxiRepository;
        }

        public void RegisterTaxi(Taxi taxi, Location currentLocation, TaxiAvailabilityPreferences taxiAvailabilityPreferences)
        {
            var availableTaxi = new AvailableTaxi(taxi, currentLocation, taxiAvailabilityPreferences);
            availableTaxiRepository.Save(availableTaxi);
        }

        public string BookTaxi(Taxi taxi, Customer customer, Location startLocation, Location endLocation, double price)
        {
            try
            {
                availableTaxiRepository.FindById(taxi.Id);
            }
            catch (NotFoundTaxi)
            {
                throw new AlreadyBookedTaxi();
            }
            return new Guid().ToString();
        }
    }
}
