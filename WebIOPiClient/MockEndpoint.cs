using System;
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

        public Task<int> OutputBitSequence(int gpioNumber, int period, string bits)
        {
            if (string.IsNullOrEmpty(bits))
                throw new ArgumentException("Empty bit stream.", nameof(bits));
            if (bits.Any(b => !new[] { '0', '1' }.Contains(b)))
                throw new ArgumentException("Invalid bit sequence.", nameof(bits));
            if (period < 0)
                throw new ArgumentException("Delay period must be non-negative.", nameof(period));
            return Task.FromResult(int.Parse(bits.Last().ToString()));
        }

        public Task<string> OutputPWM(int gpioNumber, float pulseRatio)
        {
            if (pulseRatio < 0.0f || pulseRatio > 1.0f)
                throw new ArgumentException("Pulse ratio out of range [0.0 - 1.0].", nameof(pulseRatio));
            return Task.FromResult("result");
        }

        public Task<string> OutputPWM(int gpioNumber, int angle)
        {
            if (angle < -45 || angle > 45)
                throw new ArgumentException("Pulse angle out of range [-45 - +45].", nameof(angle));
            return Task.FromResult("result");
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

        private const string ExampleStatus = 
            @"{""UART0"": 1, ""I2C0"": 0, ""I2C1"": 1, ""SPI0"": 0, ""GPIO"":{
                    ""0"": { ""function"": ""IN"", ""value"": 1}, 
                    ""1"": { ""function"": ""IN"", ""value"": 1}, 
                    ""2"": { ""function"": ""ALT0"", ""value"": 1}, 
                    ""3"": { ""function"": ""ALT0"", ""value"": 1}, 
                    ""4"": { ""function"": ""IN"", ""value"": 0}, 
                    ""5"": { ""function"": ""ALT0"", ""value"": 0}, 
                    ""6"": { ""function"": ""OUT"", ""value"": 1}, 
                    ""53"": { ""function"": ""ALT3"", ""value"": 1}
                 }}";

        public Task<GPIOState> GetGPIOState()
        {
            return Task.FromResult(GPIOState.FromJSONString(ExampleStatus));
        }

        public Task<string> GetGPIOStateJSON()
        {
            return Task.FromResult(ExampleStatus);
        }

        private class GPIO
        {
            public GPIOFunctions Function { get; set; }

            public int PinNumber { get; set; }

            public int Value { get; set; }
        }
    }
}