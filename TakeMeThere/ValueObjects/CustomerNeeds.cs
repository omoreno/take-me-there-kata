namespace TakeMeThere.ValueObjects
{
    public class CustomerNeeds
    {
        public readonly TaxiSize Size;
        public readonly int NumberOfSeats;
        public readonly bool WithAirConditioned;
        public readonly bool WheelchairAccessible;
        public readonly bool WithExtraBagaggeSpace;
        public readonly bool WithLuxuriousEquipment;

        public CustomerNeeds(TaxiSize size, int numberOfSeats, bool withAirConditioned, bool wheelchairAccessible, bool withExtraBagaggeSpace, bool withLuxuriousEquipment)
        {
            Size = size;
            NumberOfSeats = numberOfSeats;
            WithAirConditioned = withAirConditioned;
            WheelchairAccessible = wheelchairAccessible;
            WithExtraBagaggeSpace = withExtraBagaggeSpace;
            WithLuxuriousEquipment = withLuxuriousEquipment;
        }
    }
}