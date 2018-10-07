using System;
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

        public async Task<GPIOFunctions> GetGPIOFunction(int gpioNumber)
        {
            var response = await _client.GetAsync(GetFullUrl($"/GPIO/{gpioNumber}/function"));
            response.EnsureSuccessStatusCode();
            return GetFunctionFromString(await response.Content.ReadAsStringAsync());
        }

        public async Task<GPIOFunctions> SetGPIOFunction(int gpioNumber, GPIOFunctions function)
        {
            var response = await _client.PostAsync(GetFullUrl($"/GPIO/{gpioNumber}/function/{GetStringFromFunction(function)}"), null);
            response.EnsureSuccessStatusCode();
            return GetFunctionFromString(await response.Content.ReadAsStringAsync());
        }

        public async Task<int> GetGPIOValue(int gpioNumber)
        {
            var response = await _client.GetAsync(GetFullUrl($"/GPIO/{gpioNumber}/value"));
            response.EnsureSuccessStatusCode();
            return int.Parse(await response.Content.ReadAsStringAsync());
        }

        public async Task<int> SetGPIOValue(int gpioNumber, int value)
        {
            var response = await _client.PostAsync(GetFullUrl($"/GPIO/{gpioNumber}/value/{value}"), null);
            response.EnsureSuccessStatusCode();
            return int.Parse(await response.Content.ReadAsStringAsync());
        }

        public async Task<int> OutputSinglePulse(int gpioNumber)
        {
            var response = await _client.PostAsync(GetFullUrl($"/GPIO/{gpioNumber}/pulse/"), null);
            response.EnsureSuccessStatusCode();
            return int.Parse(await response.Content.ReadAsStringAsync());
        }

        public async Task<string> RunMacro(string macroName)
        {
            var response = await _client.PostAsync(GetFullUrl($"/macros/{macroName}"), null);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        private string GetFullUrl(string route)
        {
            return _deviceEndpointUrl + route;
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

        #region IDisposable Support

        private bool _disposedValue;

        private void Dispose(bool disposing)
        {
            if (_disposedValue) return;
            if (disposing)
            {
                _client?.Dispose();
            }
            _disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
