﻿using System;

namespace TakeMeThere.Models
{
    public class Taxi
    {
        public string Id { get; private set; }
        public TaxiSize Size { get; private set; }
        public int NumberOfSeats { get; private set; }
        public bool AirConditioned { get; private set; }
        public bool WheelchairAccesible { get; private set; }
        public bool ExtraBaggageSpace { get; private set; }
        public bool LuxuriousEquipment { get; private set; }
        public int Price { get; private set; }

        public Taxi(TaxiSize size, int numberOfSeats, bool airConditioned, bool wheelchairAccesible, bool extraBaggageSpace, bool luxuriousEquipment)
        {
            Id = new Guid().ToString();
            Size = size;
            NumberOfSeats = numberOfSeats;
            AirConditioned = airConditioned;
            WheelchairAccesible = wheelchairAccesible;
            ExtraBaggageSpace = extraBaggageSpace;
            LuxuriousEquipment = luxuriousEquipment;
            Price = CalculatePrice();
        }

        private int CalculatePrice()
        {
            return (int)Size + NumberOfSeats +
                    (AirConditioned ? 1 : 0) +
                    (WheelchairAccesible ? 1 : 0) +
                    (ExtraBaggageSpace ? 1 : 0) +
                    (LuxuriousEquipment ? 1 : 0);
        }
    }
}