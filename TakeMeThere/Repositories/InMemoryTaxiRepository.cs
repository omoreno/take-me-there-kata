using System.Collections.Generic;
using System.Linq;
using TakeMeThere.Models;

namespace TakeMeThere.Repositories
{
    public class InMemoryTaxiRepository : ITaxiRepository
    {
        private readonly List<Taxi> availableTaxis; 

        public InMemoryTaxiRepository()
        {
            if (availableTaxis == null)
                availableTaxis = new List<Taxi>();
        }

        public void Save(Taxi taxi)
        {
            availableTaxis.Add(taxi);
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

        public List<Taxi> GetAll()
        {
            return availableTaxis;
        }

        public void Update(Taxi taxi)
        {
            availableTaxis.Remove(taxi);
            Save(taxi);
        }
    }
}