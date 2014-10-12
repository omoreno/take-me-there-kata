using TakeMeThere.Models;

namespace TakeMeThere.Repositories
{
    public interface IAvailableTaxiRepository
    {
        void Save(AvailableTaxi availableTaxi);
        bool Exists(string taxiId);
        void Delete(string taxiId);
    }
}