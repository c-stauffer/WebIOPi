using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebIOPiClient
{
    public sealed class MockEndpoint : IEndpoint
    {
        private List<GPIO> _list;

        public MockEndpoint()
        {
            _list = new List<GPIO>();
            for (int i = 2; i < 28; i++)
            {
                _list.Add(new GPIO { PinNumber = i, Value = 0, Function = GPIOFunctions.In });
            };
        }

        public void Dispose()
        {
            _list = null;
        }

        /// <summary>
        /// Gets the function type of the specified pin
        /// </summary>
        /// <param name="gpioNumber"> GPIO Pin Number </param>
        public Task<GPIOFunctions> GetGPIOFunction(int gpioNumber)
        {
            return Task.FromResult(_list.Single(p => p.PinNumber == gpioNumber).Function);//.Result;
        }

        /// <summary>
        /// Gets the value of the specified pin
        /// </summary>
        /// <param name="gpioNumber"> GPIO Pin Number </param>
        public Task<int> GetGPIOValue(int gpioNumber)
        {
            return Task.FromResult(_list.Single(p => p.PinNumber == gpioNumber).Value);
        }

        /// <summary>
        /// Sends a single pulse to the specified pin
        /// </summary>
        /// <param name="gpioNumber"> GPIO Pin Number </param>
        public Task<int> OutputSinglePulse(int gpioNumber)
        {
            // not actually pulsing a fake device, just return pin's value.
            return GetGPIOValue(gpioNumber);
        }

        /// <summary>
        /// Runs the specified macro
        /// </summary>
        /// <param name="macroName"> Macro Name </param>
        public Task<string> RunMacro(string macroName)
        {
            return Task.FromResult("macro run");
        }

        /// <summary>
        /// Sets the function type of the specified pin
        /// </summary>
        /// <param name="gpioNumber"> GPIO Pin Number </param>
        /// <param name="function">   Function type </param>
        public Task<GPIOFunctions> SetGPIOFunction(int gpioNumber, GPIOFunctions function)
        {
            _list.Single(p => p.PinNumber == gpioNumber).Function = function;
            return Task.FromResult(_list.Single(p => p.PinNumber == gpioNumber).Function);
        }

        /// <summary>
        /// Sets the value of the specified pin
        /// </summary>
        /// <param name="gpioNumber"> GPIO Pin Number </param>
        /// <param name="value">      Value to set to </param>
        public Task<int> SetGPIOValue(int gpioNumber, int value)
        {
            _list.Single(p => p.PinNumber == gpioNumber).Value = value;
            return Task.FromResult(_list.Single(p => p.PinNumber == gpioNumber).Value);
        }

        private class GPIO
        {
            public GPIOFunctions Function { get; set; }

            public int PinNumber { get; set; }

            public int Value { get; set; }
        }
    }
}