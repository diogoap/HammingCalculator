using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("HammingCalculator.Lib.Tests")]
/// <summary>
/// Calculates the Hamming Distance of two arrays of bytes.
/// Calculation is performed in a single thread.
/// The complexity is O(n) since the execution time is proportional to the array's size.
/// </summary>
namespace HammingCalculator.Lib.DistanceCalculator
{
    internal class HammingDistanceCalculatorStandard : IHammingDistanceCalculatorStrategy
    {
        /// <summary>
        /// Calculates the Hamming Distance of the two arrays of bytes.
        /// </summary>
        /// <param name="input1">First input</param>
        /// <param name="input2">Second input</param>
        /// <returns>Total distance between the two arrays of bytes</returns>
        public int Calculate(byte[] input1, byte[] input2)
        {
            int distance = 0;

            for (int i = 0; i < input1.Length; i++)
            {
                if (input1[i] != input2[i])
                {
                    distance++;
                }
            }

            return distance;
        }
    }
}
