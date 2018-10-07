using System;
using System.Threading.Tasks;

namespace WebIOPiClient
{
    public interface IEndpoint : IDisposable
    {
        Task<GPIOFunctions> GetGPIOFunction(int gpioNumber);
        Task<int> GetGPIOValue(int gpioNumber);
        Task<int> OutputSinglePulse(int gpioNumber);
        Task<GPIOFunctions> SetGPIOFunction(int gpioNumber, GPIOFunctions function);
        Task<int> SetGPIOValue(int gpioNumber, int value);
        Task<string> RunMacro(string macroName);
    }
}