using System.Collections.Generic;
using TakeMeThere.Models;

namespace TakeMeThere.Repositories
{
    public class InMemoryAvailableTaxiRepository : IAvailableTaxiRepository
    {
        public void Save(AvailableTaxi availableTaxi)
        {
            throw new System.NotImplementedException();
        }

        public bool Exists(string taxiId)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(string taxiId)
        {
            throw new System.NotImplementedException();
        }

        public List<AvailableTaxi> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public void Update(AvailableTaxi availableTaxi)
        {
            throw new System.NotImplementedException();
        }
    }
}