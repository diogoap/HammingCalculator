using HammingCalculator.Lib;
using System;
using System.Diagnostics;

namespace HammingCalculator.App
{
    /// <summary>
    /// Hamming Calculator Program
    /// Arguments expected:
    ///     #1: Source of data: argument name must be either -inline or -file
    ///         * Required 2 values
    ///         -> If source is -inline:
    ///             -> Value must be two strings containing a sequence of bits. Example: "0101010101010101".
    ///             -> Strings must be equal size.
    ///             
    ///         -> If source is -file:
    ///             -> Value must be two valid file names. Example: "C:\file.txt".
    ///             -> File contents must be equal size.
    ///             
    ///     #2: Method of caculation: argument name is -method
    ///         * Optional
    ///         -> Value should be
    ///             -> Standard: default calculation method
    ///             -> Parallel: calculation with paralelism. Should be used in large amounts of data.
    /// 
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            // Parses all program arguments into ProgramOptions object
            var options = new ProgramOptions(args);

            //Creates a Hamming Calculator instance basead on the current strategy
            var hammingCalculator = new HammingCalculatorHelper(options.CalculatorStrategy);

            //Calculates the Hamming Distance according to the 2 inputs
            var watch = Stopwatch.StartNew();
            var distance = hammingCalculator.CalculateDistance(options.InputsBytes.Item1, options.InputsBytes.Item2);
            watch.Stop();

            // Writes the calculation inputs and results to console
            Console.WriteLine("Calculating Hamming Distance for the inputs below:");
            Console.WriteLine($"Input 1: {options.Inputs.Item1}");
            Console.WriteLine($"Input 2: {options.Inputs.Item2}");
            Console.WriteLine($"Strategy: {options.CalculatorStrategy.ToString()}");
            Console.WriteLine();
            Console.WriteLine($"Hamming distance for the provided inputs is {distance}.");
            Console.WriteLine();
            Console.WriteLine($"Elapsed Milliseconds: {watch.ElapsedMilliseconds}");
        }
    }
}
