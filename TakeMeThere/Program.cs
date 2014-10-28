using System;
using System.Diagnostics;
using System.Text;
using TakeMeThere.Models;
using TakeMeThere.ValueObjects;

namespace TakeMeThere
{
    class Program
    {
        private static readonly Random Random = new Random();
        static void Main(string[] args)
        {
            var cli = Factory.CommandLineInterface();
            Console.WriteLine("Creating 1 million Taxis...");
            var watch = Stopwatch.StartNew();
            Create1MTaxis(cli);
            watch.Stop();
            Console.WriteLine("Time (ms):" + watch.ElapsedMilliseconds);
            Console.WriteLine("------------------------------------------------");

            Console.WriteLine("Creating 1 million Customers...");
            watch = Stopwatch.StartNew();
            var customer = Create1MCustomers(cli);
            watch.Stop();
            Console.WriteLine("Time (ms):" + watch.ElapsedMilliseconds);
            Console.WriteLine("------------------------------------------------");

            Console.WriteLine("Searching...");
            watch = Stopwatch.StartNew();
            var taxis = cli.GetTaxis(customer, new Location(1, 1), TaxiSearchFilter.BestRated,
                         new CustomerNeeds(TaxiSize.Small, 4, true, false, true, true));
            watch.Stop();
            Console.WriteLine("Time (ms):" + watch.ElapsedMilliseconds);
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine("10 most promising taxi Ids:");
            foreach (var taxi in taxis)
                Console.WriteLine(taxi.Id);
        }

        private static void Create1MTaxis(CommandLineInterface cli)
        {
            for (var i = 0; i < 1000000; i++)
                cli.RegisterTaxi(new TaxiFeatures(RandomTaxiSize(), 4, RandomBoolean(), RandomBoolean(), RandomBoolean(), RandomBoolean()), new Location(100, 100), new TaxiAvailabilityPreferences(TaxiTripLength.Short, null, 10000));
        }

        private static Customer Create1MCustomers(CommandLineInterface cli)
        {
            var customer = new Customer(new CustomerPreferences(null));
            for (var i = 0; i < 1000000; i++)
            {
                customer = new Customer(new CustomerPreferences(null));
                cli.RegisterCustomer(customer);
            }
            return customer;
        }

        private static TaxiSize RandomTaxiSize()
        {
            return (TaxiSize)Random.Next(0, 3);
        }

        private static bool RandomBoolean()
        {
            return Random.Next(0, 2) == 0;
        }
    }
}
