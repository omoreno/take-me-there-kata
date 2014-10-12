using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using NUnit.Framework;

namespace TakeMeThere
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void ShouldRegisterNewAvailableTaxi()
        {
            var availableTaxiRepository = new Mock<IAvailableTaxiRepository>();
            var api = new Api(availableTaxiRepository.Object);
            var taxi = new Taxi(size: TaxiSize.Small, numberOfSeats: 4, airConditioned: true,
                                wheelchairAccesible: false, extraBaggageSpace: false,
                                luxuriousEquipment: false);
            var taxiAvailabilityPreferences = new TaxiAvailabilityPreferences(TaxiTripLength.Short, 3, 10000);

            api.RegisterTaxi(taxi, new Location(0, 0), taxiAvailabilityPreferences);

            availableTaxiRepository.Verify(x => x.Save(It.IsAny<AvailableTaxi>()));
        }

        public class TaxiAvailabilityPreferences
        {
            public TaxiTripLength TripLength { get; private set; }
            public int MinimunCustomerRating { get; private set; }
            public int WorkingLocationRadio { get; private set; }

            public TaxiAvailabilityPreferences(TaxiTripLength tripLength, int minimunCustomerRating, int workingLocationRadio)
            {
                TripLength = tripLength;
                MinimunCustomerRating = minimunCustomerRating;
                WorkingLocationRadio = workingLocationRadio;
            }
        }

        public class Taxi
        {
            public TaxiSize Size { get; private set; }
            public int NumberOfSeats { get; private set; }
            public bool AirConditioned { get; private set; }
            public bool WheelchairAccesible { get; private set; }
            public bool ExtraBaggageSpace { get; private set; }
            public bool LuxuriousEquipment { get; private set; }

            public Taxi(TaxiSize size, int numberOfSeats, bool airConditioned, bool wheelchairAccesible, bool extraBaggageSpace, bool luxuriousEquipment)
            {
                Size = size;
                NumberOfSeats = numberOfSeats;
                AirConditioned = airConditioned;
                WheelchairAccesible = wheelchairAccesible;
                ExtraBaggageSpace = extraBaggageSpace;
                LuxuriousEquipment = luxuriousEquipment;
            }

        }

        public enum TaxiSize
        {
            Small,
            Medium,
            Large
        }

        public enum TaxiTripLength
        {
            Short,
            Long
        }

        public class Location
        {
            private double longitude;
            private double latitude;

            public Location(double latitude, double longitude)
            {
                this.latitude = latitude;
                this.longitude = longitude;
            }
        }
    }

    public class AvailableTaxi
    {
        private readonly Tests.Taxi _taxi;
        private readonly Tests.Location _currentLocation;
        private readonly Tests.TaxiTripLength tripLength;
        private readonly int minimunCustomerRating;
        private readonly int workingLocationRadio;

        public AvailableTaxi(Tests.Taxi taxi, Tests.Location currentLocation, Tests.TaxiAvailabilityPreferences taxiAvailabilityPreferences)
        {
            _taxi = taxi;
            _currentLocation = currentLocation;
            tripLength = taxiAvailabilityPreferences.TripLength;
            minimunCustomerRating = taxiAvailabilityPreferences.MinimunCustomerRating;
            workingLocationRadio = taxiAvailabilityPreferences.WorkingLocationRadio;
        }
    }

    public class Api
    {
        private readonly IAvailableTaxiRepository availableTaxiRepository;

        public Api(IAvailableTaxiRepository availableTaxiRepository)
        {
            this.availableTaxiRepository = availableTaxiRepository;
        }

        public void RegisterTaxi(Tests.Taxi taxi, Tests.Location currentLocation, Tests.TaxiAvailabilityPreferences taxiAvailabilityPreferences)
        {
            var availableTaxi = new AvailableTaxi(taxi, currentLocation, taxiAvailabilityPreferences);
            availableTaxiRepository.Save(availableTaxi);
        }
    }

    public interface IAvailableTaxiRepository
    {
        void Save(AvailableTaxi availableTaxi);
    }
}
