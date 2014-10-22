using TakeMeThere.Models;
using TakeMeThere.Repositories;

namespace TakeMeThere.Services
{
    public class TaxiRegisterService
    {
        private readonly ITaxiRepository taxiRepository;

        public TaxiRegisterService(ITaxiRepository taxiRepository)
        {
            this.taxiRepository = taxiRepository;
        }

        public void RegisterTaxi(Taxi taxi)
        {
            taxiRepository.Save(taxi);
        }
    }
}