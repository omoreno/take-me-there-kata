﻿using System;
using System.Diagnostics;
using NUnit.Framework;
using TakeMeThere.Models;
using TakeMeThere.ValueObjects;

namespace TakeMeThere.Tests
{
    [TestFixture]
    public class PerformanceTests
    {
        private CommandLineInterface api;
        private Random random;

        [SetUp]
        public void SetUp()
        {
            api = Factory.CommandLineInterface();
            random = new Random();
        }

        [Test]
        public void ShouldFindTaxisInLessThanASecond()
        {
            Create1MillionTaxis();
            var customer = Create1MillionCustomers();

            var watch = Stopwatch.StartNew();
            api.GetTaxis(customer, new Location(1, 1), TaxiSearchFilter.BestRated,
                             new CustomerNeeds(TaxiSize.Small, 4, true, false, true, true));
            watch.Stop();

            Assert.LessOrEqual(watch.ElapsedMilliseconds, 1000);
        }

        private void Create1MillionTaxis()
        {
            for (var i = 0; i < 1000000; i++)
                api.RegisterTaxi(new TaxiFeatures(RandomTaxiSize(), 4, RandomBoolean(), RandomBoolean(), RandomBoolean(), RandomBoolean()), new Location(100, 100), new TaxiAvailabilityPreferences(TaxiTripLength.Short, null, 10000));
        }

        private TaxiSize RandomTaxiSize()
        {
            return (TaxiSize) random.Next(0, 3);
        }
        
        private bool RandomBoolean()
        {
            return random.Next(0, 2) == 0;
        }

        private Customer Create1MillionCustomers()
        {
            var customer = new Customer(new CustomerPreferences(null));
            for (var i = 0; i < 1000000; i++)
            {
                customer = new Customer(new CustomerPreferences(null));
                api.RegisterCustomer(customer);
            }
            return customer;
        }
    }
}