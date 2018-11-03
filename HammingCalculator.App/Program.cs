using HammingCalculator.Lib;
using HammingCalculator.Lib.DistanceCalculator;
using System;
using System.Text;

namespace HammingCalculator.App
{
    /// <summary>
    /// Hamming Calculator Program
    /// Arguments expected:
    ///     #1: String containing a sequence of bits. Example: "0101010101010101".
    ///     #2: String containing a sequence of bits. Example: "0101010101010101".
    ///     #3: String containing the calculation strategy. Example: "0" to use Standard method or "1" to use Parallel method.
    /// 
    /// Arguments #1 and #2 are mandatory and should be equal size.
    /// Argument #3 is optional. Standard method is used if argument is not provided.
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                throw new ArgumentException("Invalid arguments");
            }

            // Converts the string arguments into array of bytes
            // Assumption: the inputs will be strings with binary data (0s and 1s)
            var input1 = Encoding.ASCII.GetBytes(args[0]);
            var input2 = Encoding.ASCII.GetBytes(args[1]);

            //If provided, the 3rd argument is converted into the correspondent Hamming Distance Calculator strategy
            var calculatorStrategy = GetHammingDistanceCalculatorStrategy(args);

            //Creates a Hamming Calculator instance basead on the current strategy
            var hammingCalculator = new HammingCalculatorHelper(calculatorStrategy);

            //Calculates the Hamming Distance according to the 2 inputs
            var distance = hammingCalculator.CalculateDistance(input1, input2);

            // Writes the calculation inputs and results to console
            Console.WriteLine("Calculating Hamming Distance for the inputs below:");
            Console.WriteLine($"Input 1: {args[0]}");
            Console.WriteLine($"Input 2: {args[1]}");
            Console.WriteLine($"Strategy: {calculatorStrategy.ToString()}");
            Console.WriteLine();
            Console.WriteLine($"Hamming distance for the provided inputs is {distance}.");
        }

        /// <summary>
        /// Instantiates the proper calculator strategy based on the argument provided to the application.
        /// The 3rd argument is considered as the desired strategy to be used.
        /// If the 3rd argument is not provided or is not recognized, Standard strategy is adopted.
        /// </summary>
        /// <param name="args">Arguments provided to the console application</param>
        /// <returns>An instance of the selected IHammingDistanceCalculatorStrategy</returns>
        private static IHammingDistanceCalculatorStrategy GetHammingDistanceCalculatorStrategy(string[] args)
        {
            if (args.Length >= 3)
            {
                if (Enum.TryParse(typeof(HammingDistanceCalculatorStrategies), args[2], out object strategyInput))
                {
                    return HammingDistanceCalculatorFactory.CreateInstance((HammingDistanceCalculatorStrategies)strategyInput);
                }
            }
            return HammingDistanceCalculatorFactory.CreateInstance(HammingDistanceCalculatorStrategies.Standard);
        }
    }
}
