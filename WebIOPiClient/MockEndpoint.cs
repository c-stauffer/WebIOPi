using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebIOPiClient
{
    public sealed class MockEndpoint : IEndpoint
    {
        private class GPIO
        {
            public int PinNumber { get; set; }
            public int Value { get; set; }
            public GPIOFunctions Function { get; set; }
        }


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

        public Task<GPIOFunctions> GetGPIOFunction(int gpioNumber)
        {
            return Task.FromResult(_list.Single(p => p.PinNumber == gpioNumber).Function);//.Result;
        }

        public Task<int> GetGPIOValue(int gpioNumber)
        {
            return Task.FromResult(_list.Single(p => p.PinNumber == gpioNumber).Value);
        }

        public Task<int> OutputSinglePulse(int gpioNumber)
        {
            // not actually pulsing a fake device, just return pin's value.
            return GetGPIOValue(gpioNumber);
        }

        public Task<GPIOFunctions> SetGPIOFunction(int gpioNumber, GPIOFunctions function)
        {
            _list.Single(p => p.PinNumber == gpioNumber).Function = function;
            return Task.FromResult(_list.Single(p => p.PinNumber == gpioNumber).Function);
        }

        public Task<int> SetGPIOValue(int gpioNumber, int value)
        {
            _list.Single(p => p.PinNumber == gpioNumber).Value = value;
            return Task.FromResult(_list.Single(p => p.PinNumber == gpioNumber).Value);
        }

        public Task<string> RunMacro(string macroName)
        {
            return Task.FromResult("macro run");
        }
    }
}
