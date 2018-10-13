using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace WebIOPiClient
{
    public sealed class GPIOState
    {
        /// <summary>
        /// Gets a value indicating whether UART0 is enabled on the device
        /// </summary>
        public bool UART0_Enabled { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether I2C0 is enabled on the device
        /// </summary>
        public bool I2C0_Enabled { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether I2C1 is enabled on the device
        /// </summary>
        public bool I2C1_Enabled { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether SPI0 is enabled on the device
        /// </summary>
        public bool SPI0_Enabled { get; internal set; }

        /// <summary>
        /// Gets a sequence of pin statuses indexed by pin number.
        /// </summary>
        public List<PinDetails> PinStatuses { get; } = new List<PinDetails>();

        internal GPIOState() { }

        /// <summary>
        /// Builds and returns a <see cref="GPIOState"/> from the JSON
        /// string returned from the device (GET /*).
        /// </summary>
        /// <param name="json">JSON configuration/status information</param>
        /// <returns>A <see cref="GPIOState"/> containing the values from the JSON string</returns>
        internal static GPIOState FromJSONString(string json)
        {
            //TODO: This code is based off the example from the WebIOPi documentation.
            //      Need some more real-world examples to verify that this works in all cases/configurations.
            var parsedObject = new GPIOState();
            var parser = JObject.Parse(json);
            parsedObject.UART0_Enabled = (int)parser["UART0"] == 1;
            parsedObject.I2C0_Enabled = (int)parser["I2C0"] == 1;
            parsedObject.I2C1_Enabled = (int)parser["I2C1"] == 1;
            parsedObject.SPI0_Enabled = (int)parser["SPI0"] == 1;

            // pins are *not* in a json array but an object:
            // "GPIO":{
            //    "0": { "function": "IN", "value": 1}, 
            //    "1": { "function": "IN", "value": 1}, 
            // etc...
            var gpio = (JObject)parser["GPIO"];
            for (int i = 0; i < 54; i++)
            {
                var pinItem = gpio[i.ToString()];
                parsedObject.PinStatuses.Add(new PinDetails
                {
                    PinNumber = i,
                    Function = (string)pinItem["function"],
                    Value = (int)pinItem["value"]
                });
            }
            return parsedObject;
        }

        /// <summary>
        /// Returns a <see cref="PinDetails"/> instance representing the status of the requested
        /// pin. If the requested pin number is not valid for this device, an exception is thrown.
        /// </summary>
        /// <param name="gpioNumber"></param>
        /// <returns>Pin information for pin at requested index.</returns>
        public PinDetails GetDetailsForPin(int gpioNumber)
        {
            // this will throw exception if requested pin number is not found.
            return PinStatuses.First(p => p.PinNumber == gpioNumber);
        }

        public struct PinDetails
        {
            /// <summary>
            /// Gets the pin number of this instance.
            /// </summary>
            public int PinNumber { get; internal set; }

            /// <summary>
            /// Gets the current Function of the pin.
            /// </summary>
            public string Function { get; internal set; }

            /// <summary>
            /// Gets the current Value of the pin.
            /// </summary>
            public int Value { get; internal set; }
        }
    }
}
