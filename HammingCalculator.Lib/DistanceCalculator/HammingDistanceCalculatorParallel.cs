using System.Threading.Tasks;

namespace HammingCalculator.Lib.DistanceCalculator
{
    /// <summary>
    /// Calculates the Hamming Distance of two arrays of bytes.
    /// Calculation is performed in a parallel mode, which means the data is split into small chunks of data.
    /// Then each chunk is processed in different threads to use more than one processor and run faster.
    /// The complexity is O(n) since the execution time is proportional to the array's size.
    /// However, the more processors and cores are available, lesser will be the total execution time.
    /// </summary>
    internal class HammingDistanceCalculatorParallel : IHammingDistanceCalculatorStrategy
    {
        private const int ChunkSize = 100_000;
        private readonly object _distanceLock = new object();

        /// <summary>
        /// Calculates the Hamming Distance of the two arrays of bytes.
        /// </summary>
        /// <param name="input1">First input</param>
        /// <param name="input2">Second input</param>
        /// <returns>Total distance between the two arrays of bytes</returns>
        public int Calculate(byte[] input1, byte[] input2)
        {
            int distanceTotal = 0;
            int chunkCount = GetNumberOfChunks(input1.Length);

            Parallel.For(0, chunkCount, currentChunk =>
            {
                int distancePartial = CalculatePartial(input1, input2, currentChunk);

                lock (_distanceLock) 
                {
                    distanceTotal += distancePartial;
                }
            });

            return distanceTotal;           
        }

        /// <summary>
        /// Calculates the Hamming Distance of the two arrays of bytes.
        /// Distance is calculated only in the range related to the currentChunk argument.
        /// </summary>
        /// <param name="input1">First input</param>
        /// <param name="input2">Second input</param>
        /// <param name="currentChunk">Indicates the current chunk of data to be processed</param>
        /// <returns>Partial distance calculated for the current chunk</returns>
        private int CalculatePartial(byte[] input1, byte[] input2, int currentChunk)
        {
            int distance = 0;
            int startPos = currentChunk * ChunkSize;
            int endPos = (startPos + ChunkSize) - 1;    
            int effectiveEnd = endPos >= input1.Length ? input1.Length - 1 : endPos;

            for (int i = startPos; i <= effectiveEnd; i++)
            {
                if (input1[i] != input2[i])
                {
                    distance++;
                }
            }
            return distance;
        }

        /// <summary>
        /// Calculates the number of chunks of data which inputs will be split.
        /// </summary>
        /// <param name="inputLength">Total length of the input</param>
        /// <returns>Number of chunks of data</returns>
        private int GetNumberOfChunks(int inputLength)
        {
            if (inputLength <= ChunkSize)
            {
                return 1;
            }
            else
            {
                return inputLength / ChunkSize;
            }
        }
    }
}
