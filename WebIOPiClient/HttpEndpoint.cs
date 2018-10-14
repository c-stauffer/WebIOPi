using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebIOPiClient
{
    public sealed class HttpEndpoint : IEndpoint
    {
        private readonly HttpClient _client;
        private readonly string _deviceEndpointUrl;

        public HttpEndpoint(string deviceEndpointUrl, string username, string password)
        {
            _deviceEndpointUrl = deviceEndpointUrl;
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("Authorization", GetAuthToken(username, password));
        }

        /// <summary>
        /// Gets the function type of the specified pin
        /// </summary>
        /// <param name="gpioNumber"> GPIO Pin Number </param>
        public async Task<GPIOFunctions> GetGPIOFunction(int gpioNumber)
        {
            var response = await _client.GetAsync(GetFullUrl($"/GPIO/{gpioNumber}/function"));
            response.EnsureSuccessStatusCode();
            return GetFunctionFromString(await response.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Sets the function type of the specified pin
        /// </summary>
        /// <param name="gpioNumber"> GPIO Pin Number </param>
        /// <param name="function">   Function type </param>
        public async Task<GPIOFunctions> SetGPIOFunction(int gpioNumber, GPIOFunctions function)
        {
            var response = await _client.PostAsync(GetFullUrl($"/GPIO/{gpioNumber}/function/{GetStringFromFunction(function)}"), null);
            response.EnsureSuccessStatusCode();
            return GetFunctionFromString(await response.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Gets the value of the specified pin
        /// </summary>
        /// <param name="gpioNumber"> GPIO Pin Number </param>
        public async Task<int> GetGPIOValue(int gpioNumber)
        {
            var response = await _client.GetAsync(GetFullUrl($"/GPIO/{gpioNumber}/value"));
            response.EnsureSuccessStatusCode();
            return int.Parse(await response.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Sets the value of the specified pin
        /// </summary>
        /// <param name="gpioNumber"> GPIO Pin Number </param>
        /// <param name="value">      Value to set to </param>
        public async Task<int> SetGPIOValue(int gpioNumber, int value)
        {
            var response = await _client.PostAsync(GetFullUrl($"/GPIO/{gpioNumber}/value/{value}"), null);
            response.EnsureSuccessStatusCode();
            return int.Parse(await response.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Sends a single pulse to the specified pin
        /// </summary>
        /// <param name="gpioNumber"> GPIO Pin Number </param>
        public async Task<int> OutputSinglePulse(int gpioNumber)
        {
            var response = await _client.PostAsync(GetFullUrl($"/GPIO/{gpioNumber}/pulse/"), null);
            response.EnsureSuccessStatusCode();
            return int.Parse(await response.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Runs the specified macro
        /// </summary>
        /// <param name="macroName"> Macro Name </param>
        public async Task<string> RunMacro(string macroName)
        {
            var response = await _client.PostAsync(GetFullUrl($"/macros/{macroName}"), null);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Outputs a PWM duty cycle ratio on the specified pin
        /// </summary>
        /// <param name="gpioNumber">GPIO pin number</param>
        /// <param name="pulseRatio">Duty cycle, in range 0.0 to 1.0</param>
        /// <returns>value</returns>
        public async Task<string> OutputPWM(int gpioNumber, float pulseRatio)
        {
            if (pulseRatio < 0.0f || pulseRatio > 1.0f)
                throw new ArgumentException("Pulse ratio out of range [0.0 - 1.0].", nameof(pulseRatio));
            var response = await _client.PostAsync(GetFullUrl($"/GPIO/{gpioNumber}/pulseRatio/{pulseRatio:0.0}"), null);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Outputs a PWM angle on the specified pin
        /// </summary>
        /// <param name="gpioNumber">GPIO pin number</param>
        /// <param name="angleInDegrees">Angle, in degrees, in range -45 to +45.</param>
        /// <returns>value</returns>
        public async Task<string> OutputPWM(int gpioNumber, int angle)
        {
            if (angle < -45 || angle > 45)
                throw new ArgumentException("Pulse angle out of range [-45 - +45].", nameof(angle));
            var response = await _client.PostAsync(GetFullUrl($"/GPIO/{gpioNumber}/pulseRatio/angle"), null);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Outputs the specified sequence of bits onto the specified pin, separated
        /// by the specified period of time in between each bit (in ms).
        /// </summary>
        /// <param name="gpioNumber">GPIO pin number</param>
        /// <param name="period">Time in milliseconds to wait in between each bit</param>
        /// <param name="bits">A string containing 0s and 1s representing the bit stream to send</param>
        /// <returns></returns>
        public async Task<int> OutputBitSequence(int gpioNumber, int period, string bits)
        {
            if (string.IsNullOrEmpty(bits))
                throw new ArgumentException("Empty bit stream.", nameof(bits));
            if (bits.Any(b => !new[] { '0', '1' }.Contains(b)))
                throw new ArgumentException("Invalid bit sequence.", nameof(bits));
            if (period < 0)
                throw new ArgumentException("Delay period must be non-negative.", nameof(period));

            var response = await _client.PostAsync(GetFullUrl($"/GPIO/{gpioNumber}/sequence/{period},{bits}"), null);
            response.EnsureSuccessStatusCode();
            return int.Parse(await response.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Gets the enabled status of GPIO interfaces (UART, I2C, SPI) and state of each GPIO pin
        /// on the device.
        /// </summary>
        /// <returns>GPIOState with current status/configuration.</returns>
        public async Task<GPIOState> GetGPIOState()
        {
            // After doing more investigating on this, parsing is not yet feasible... The returned
            // JSON I've been seeing differs a decent amount from the example. Not sure if it's
            // different per model of Pi device, but doing this right will require digging through
            // the WebIOPi code to figure out how it builds the response.
            
            ////var response = await _client.GetAsync(GetFullUrl($"/*"));
            ////response.EnsureSuccessStatusCode();
            ////return GPIOState.FromJSONString(await GetGPIOStateJSON());
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the enabled status of GPIO interfaces (UART, I2C, SPI) and state of each GPIO pin
        /// on the device.
        /// </summary>
        /// <returns>The raw JSON string from the device, containing the current status/configuration.</returns>
        public async Task<string> GetGPIOStateJSON()
        {
            var response = await _client.GetAsync(GetFullUrl($"/*"));
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        private static string GetAuthToken(string user, string pass)
        {
            return $"Basic {Convert.ToBase64String(Encoding.ASCII.GetBytes($"{user}:{pass}"))}";
        }

        private static GPIOFunctions GetFunctionFromString(string value)
        {
            switch (value.ToLowerInvariant())
            {
                case "in":
                    return GPIOFunctions.In;

                case "out":
                    return GPIOFunctions.Out;

                case "pwm":
                    return GPIOFunctions.PWM;

                default:
                    return GPIOFunctions.Unknown;
            }
        }

        private static string GetStringFromFunction(GPIOFunctions value)
        {
            switch (value)
            {
                case GPIOFunctions.In:
                    return "in";

                case GPIOFunctions.Out:
                    return "out";

                case GPIOFunctions.PWM:
                    return "pwm";

                default:
                    throw new ArgumentException($"Unknown GPIOFunctions enum member '{value}'");
            }
        }

        private string GetFullUrl(string route)
        {
            return _deviceEndpointUrl + route;
        }

        #region IDisposable Support

        private bool _disposedValue;

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (_disposedValue) return;
            if (disposing)
            {
                _client?.Dispose();
            }
            _disposedValue = true;
        }
        #endregion IDisposable Support
    }
}