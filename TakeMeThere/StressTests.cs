using System.Diagnostics;
using NUnit.Framework;

namespace TakeMeThere
{
    [TestFixture]
    public class StressTests
    {
        private CommandLineInterface api;

        [SetUp]
        public void SetUp()
        {
            api = Factory.CommandLineInterface();
        }

    }
}