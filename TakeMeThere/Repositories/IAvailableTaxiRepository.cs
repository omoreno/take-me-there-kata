using TakeMeThere.Models;

namespace TakeMeThere.Repositories
{
    public interface IAvailableTaxiRepository
    {
        void Save(AvailableTaxi availableTaxi);
        Taxi FindById(string taxiId);
    }
}