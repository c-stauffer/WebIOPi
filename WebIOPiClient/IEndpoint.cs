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
        /// Outputs a PWM duty cycle ratio on the specified pin
        /// </summary>
        /// <param name="gpioNumber">GPIO pin number</param>
        /// <param name="pulseRatio">Duty cycle, in range 0.0 to 1.0</param>
        /// <returns></returns>
        Task<string> OutputPWM(int gpioNumber, float pulseRatio);

        /// <summary>
        /// Outputs a PWM angle on the specified pin
        /// </summary>
        /// <param name="gpioNumber">GPIO pin number</param>
        /// <param name="angleInDegrees">Angle, in degrees, in range -45 to +45.</param>
        /// <returns></returns>
        Task<string> OutputPWM(int gpioNumber, int angle);

        /// <summary>
        /// Outputs the specified sequence of bits onto the specified pin, separated
        /// by the specified period of time in between each bit (in ms).
        /// </summary>
        /// <param name="gpioNumber">GPIO pin number</param>
        /// <param name="period">Time in milliseconds to wait in between each bit</param>
        /// <param name="bits">A string containing 0s and 1s representing the bit stream to send</param>
        /// <returns></returns>
        Task<int> OutputBitSequence(int gpioNumber, int period, string bits);

        /// <summary>
        /// Runs the specified macro
        /// </summary>
        /// <param name="macroName"> Macro Name </param>
        Task<string> RunMacro(string macroName);

        /// <summary>
        /// Gets the enabled status of GPIO interfaces (UART, I2C, SPI) and state of each GPIO pin
        /// on the device.
        /// </summary>
        /// <returns>GPIOState with current status/configuration.</returns>
        Task<GPIOState> GetGPIOState();

        /// <summary>
        /// Gets the enabled status of GPIO interfaces (UART, I2C, SPI) and state of each GPIO pin
        /// on the device.
        /// </summary>
        /// <returns>The raw JSON string from the device, containing the current status/configuration.</returns>
        Task<string> GetGPIOStateJSON();
    }
}