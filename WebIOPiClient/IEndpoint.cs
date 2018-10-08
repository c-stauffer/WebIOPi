using System;
using System.Threading.Tasks;

namespace WebIOPiClient
{
    public interface IEndpoint : IDisposable
    {
        /// <summary>
        /// Gets the function type of the specified pin
        /// </summary>
        /// <param name="gpioNumber"> GPIO Pin Number </param>
        Task<GPIOFunctions> GetGPIOFunction(int gpioNumber);

        /// <summary>
        /// Sets the function type of the specified pin
        /// </summary>
        /// <param name="gpioNumber"> GPIO Pin Number </param>
        /// <param name="function">   Function type </param>
        Task<GPIOFunctions> SetGPIOFunction(int gpioNumber, GPIOFunctions function);

        /// <summary>
        /// Gets the value of the specified pin
        /// </summary>
        /// <param name="gpioNumber"> GPIO Pin Number </param>
        Task<int> GetGPIOValue(int gpioNumber);

        /// <summary>
        /// Sets the value of the specified pin
        /// </summary>
        /// <param name="gpioNumber"> GPIO Pin Number </param>
        /// <param name="value">      Value to set to </param>
        Task<int> SetGPIOValue(int gpioNumber, int value);

        /// <summary>
        /// Sends a single pulse to the specified pin
        /// </summary>
        /// <param name="gpioNumber"> GPIO Pin Number </param>
        Task<int> OutputSinglePulse(int gpioNumber);

        /// <summary>
        /// Runs the specified macro
        /// </summary>
        /// <param name="macroName"> Macro Name </param>
        Task<string> RunMacro(string macroName);

    }
}