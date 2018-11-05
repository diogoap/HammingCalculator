using HammingCalculator.Lib.DistanceCalculator;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace HammingCalculator.App
{
    /// <summary>
    /// Represents and parses all program arguments
    /// </summary>
    public class ProgramOptions
    {
        private const string SourceInlineParam = "-inline";
        private const string SourceFileParam = "-file";
        private const string MethodParam = "-method";
        private readonly string[] _args;

        public readonly IHammingDistanceCalculatorStrategy CalculatorStrategy;
        public readonly Tuple<string, string> Inputs;
        public readonly Tuple<byte[], byte[]> InputsBytes;

        /// <summary>
        /// Default constructor.
        /// Initializes all arguments values, populating the public fields with data parsed from arguments list.
        /// </summary>
        /// <param name="args"></param>
        public ProgramOptions(string[] args)
        {
            _args = args;

            if (!args.Contains(SourceInlineParam) && !args.Contains(SourceFileParam))
            {
                throw new ArgumentException($"Either {SourceInlineParam} or {SourceFileParam} argument should be provided");
            }

            if (args.Contains(SourceInlineParam))
            {
                Inputs = GetTwoArgumentsValuesByName(SourceInlineParam);
                InputsBytes = ReadBytesFromArguments();
            }
            else
            {
                Inputs = GetTwoArgumentsValuesByName(SourceFileParam);
                InputsBytes = ReadBytesFromFiles();
            }

            string method = GetArgumentValueByName(MethodParam);
            CalculatorStrategy = GetHammingDistanceCalculatorStrategy(method);
        }

        /// <summary>
        /// Returns the argument value by its name. 
        /// </summary>
        /// <param name="arg">Name of the desired argument</param>
        /// <returns>The value of the argument or an empty string if the argment was not found</returns>
        private string GetArgumentValueByName(string arg)
        {
            var argIndex = Array.FindIndex(_args, a => a == arg);

            return argIndex == -1 ? string.Empty : GetArgumentValue(argIndex + 1, arg);
        }

        /// <summary>
        /// Returns two arguments values by its name. 
        /// </summary>
        /// <param name="arg">Name of the desired argument</param>
        /// <returns>A tuple with two argument values/returns>
        private Tuple<string, string> GetTwoArgumentsValuesByName(string arg)
        {
            var argIndex = Array.FindIndex(_args, a => a == arg);

            if (argIndex == -1)
            {
                throw new ArgumentException($"Argument {arg} was not found");
            }

            return Tuple.Create(GetArgumentValue(argIndex + 1, arg), GetArgumentValue(argIndex + 2, arg));
        }

        /// <summary>
        /// Returns the argument value by its index.
        /// </summary>
        /// <param name="index">Index of the desired argument</param>
        /// <param name="arg">Name of the desired argument</param> 
        /// <returns>The value of the argument</returns>
        private string GetArgumentValue(int index, string arg)
        {
            if (index > _args.Length - 1)
            {
                throw new ArgumentException($"Argument {arg} was not provided");
            }

            return _args[index];
        }

        /// <summary>
        /// Read the contents of the two files provided in the arguments.
        /// Assumption: the files' content will be strings with binary data (0s and 1s).
        /// </summary>
        /// <returns>A tuple with two arrays of bytes</returns>
        private Tuple<byte[], byte[]> ReadBytesFromFiles()
        {
            if (!File.Exists(Inputs.Item1))
            {
                throw new ArgumentException($"File1 {Inputs.Item1} cannot be found");
            }

            if (!File.Exists(Inputs.Item2))
            {
                throw new ArgumentException($"File2 {Inputs.Item2} cannot be found");
            }

            return Tuple.Create(File.ReadAllBytes(Inputs.Item1), File.ReadAllBytes(Inputs.Item2));
        }

        /// <summary>
        /// Converts the two string arguments into array of bytes.
        /// Assumption: the inputs will be strings with binary data (0s and 1s).
        /// </summary>
        /// <returns>A tuple with two arrays of bytes</returns>
        private Tuple<byte[], byte[]> ReadBytesFromArguments()
        {
            return Tuple.Create(Encoding.ASCII.GetBytes(Inputs.Item1), Encoding.ASCII.GetBytes(Inputs.Item2));
        }

        /// <summary>
        /// Instantiates the proper calculator strategy based on "-method" argument provided to the application.
        /// If the "-method" argument is not provided or is not recognized, Standard strategy is adopted.
        /// </summary>
        /// <param name="method">Method argument value provided to the console application</param>
        /// <returns>An instance of the selected IHammingDistanceCalculatorStrategy</returns>
        private static IHammingDistanceCalculatorStrategy GetHammingDistanceCalculatorStrategy(string method)
        {
            if (!string.IsNullOrEmpty(method))
            {
                if (Enum.TryParse(typeof(HammingDistanceStrategies), method, out object strategyInput))
                {
                    return HammingDistanceCalculatorFactory.CreateInstance((HammingDistanceStrategies)strategyInput);
                }
            }
            return HammingDistanceCalculatorFactory.CreateInstance(HammingDistanceStrategies.Standard);
        }
    }
}
