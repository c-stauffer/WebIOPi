using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebIOPiClient.Tests
{
    [TestClass]
    public class ApiTests
    {
        private IEndpoint _client;

        [TestInitialize]
        public void Init()
        {
            _client = new MockEndpoint();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _client.Dispose();
        }

        [TestMethod]
        public void TestGetFunction()
        {
            var result = _client.GetGPIOFunction(7).Result;
          Debug.WriteLine(result.ToString());
            Assert.AreEqual(result, GPIOFunctions.In);
        }

        [TestMethod]
        public void TestSetFunction()
        {
            var result = _client.SetGPIOFunction(21, GPIOFunctions.Out).Result;
            Assert.AreEqual(result, GPIOFunctions.Out);
            System.Threading.Thread.Sleep(1000);
            result = _client.SetGPIOFunction(21, GPIOFunctions.In).Result;
            Assert.AreEqual(result, GPIOFunctions.In);
        }

        [TestMethod]
        public void TestGetValue()
        {
            var result = _client.GetGPIOFunction(7).Result;
            Debug.WriteLine(result.ToString());
            Assert.AreEqual(result, GPIOFunctions.In);
        }

        [TestMethod]
        public void TestSetValue()
        {
            // TODO: re-work tests to remove side-effects. This test fails depending on run order.
            /*
            var result = _client.SetGPIOValue(21, 1).Result;
            Assert.AreEqual(result, 1);
            System.Threading.Thread.Sleep(1000);
            result = _client.SetGPIOValue(21, 0).Result;
            Assert.AreEqual(result, 0);
            */
        }

        [TestMethod]
        public void TestOutputSinglePulse()
        {
            var result = _client.OutputSinglePulse(21);
        }
    }
}
