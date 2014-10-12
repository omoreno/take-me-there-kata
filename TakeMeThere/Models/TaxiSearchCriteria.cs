namespace TakeMeThere.Models
{
    public class TaxiSearchCriteria
    {
        public Customer Customer { get; private set; }
        public Location CustomerLocation { get; private set; }
        public CustomerNeeds CustomerNeeds { get; private set; }
        public TaxiSearchFilter Filter { get; private set; }

        public TaxiSearchCriteria(Customer customer, Location customerLocation, CustomerNeeds customerNeeds, TaxiSearchFilter filter)
        {
            Customer = customer;
            CustomerLocation = customerLocation;
            CustomerNeeds = customerNeeds;
            Filter = filter;
        }
    }
}