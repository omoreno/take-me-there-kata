using TakeMeThere.Models;
using TakeMeThere.Repositories;

namespace TakeMeThere.Services
{
    public class TaxiRegisterService
    {
        private readonly IAvailableTaxiRepository availableTaxiRepository;

        public TaxiRegisterService(IAvailableTaxiRepository availableTaxiRepository)
        {
            this.availableTaxiRepository = availableTaxiRepository;
        }

        public void RegisterTaxi(Taxi taxi)
        {
            availableTaxiRepository.Save(taxi);
        }
    }
}