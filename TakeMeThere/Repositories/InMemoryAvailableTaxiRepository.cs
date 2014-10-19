using System.Collections.Generic;
using System.Linq;
using TakeMeThere.Models;

namespace TakeMeThere.Repositories
{
    public class InMemoryAvailableTaxiRepository : IAvailableTaxiRepository
    {
        private readonly List<AvailableTaxi> availableTaxis; 

        public InMemoryAvailableTaxiRepository()
        {
            if (availableTaxis == null)
                availableTaxis = new List<AvailableTaxi>();
        }

        public void Save(AvailableTaxi availableTaxi)
        {
            availableTaxis.Add(availableTaxi);
        }

        public bool Exists(string taxiId)
        {
            return availableTaxis.Any(taxi => taxi.Id == taxiId);
        }

        public void Delete(string taxiId)
        {
            var existingTaxi = availableTaxis.Single(taxi => taxi.Id == taxiId);
            availableTaxis.Remove(existingTaxi);
        }

        public List<AvailableTaxi> GetAll()
        {
            return availableTaxis;
        }

        public void Update(AvailableTaxi availableTaxi)
        {
            availableTaxis.Remove(availableTaxi);
            Save(availableTaxi);
        }
    }
}