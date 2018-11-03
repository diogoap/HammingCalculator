using HammingCalculator.Lib.DistanceCalculator;
using System;

namespace HammingCalculator.Lib
{
    /// <summary>
    /// Helper class to expose Hamming Calculator methods externally.
    /// </summary>
    public class HammingCalculatorHelper
    {
        private readonly IHammingDistanceCalculatorStrategy _hammingDistanceCalculatorStrategy;

        public HammingCalculatorHelper(IHammingDistanceCalculatorStrategy hammingDistanceCalculatorStrategy)
        {
            _hammingDistanceCalculatorStrategy = hammingDistanceCalculatorStrategy;
        }

        /// <summary>
        /// Calculates the Hamming Distance of the two arrays of bytes.
        /// </summary>
        /// <param name="input1">First input</param>
        /// <param name="input2">Second input</param>
        /// <returns>Total distance between the two arrays of bytes</returns>
        public int CalculateDistance(byte[] input1, byte[] input2)
        {
            if (input1 == null || input2 == null)
            {
                throw new ArgumentNullException($"Arguments {nameof(input1)} and {nameof(input2)} cannot be null");
            }

            if (input1.Length != input2.Length)
            {
                throw new ArgumentException("Inputs must be equal length");
            }

            return _hammingDistanceCalculatorStrategy.Calculate(input1, input2);
        }
    }
}
