namespace HammingCalculator.Lib.DistanceCalculator
{
    /// <summary>
    /// Factory responbsible to instantiate a IHammingDistanceCalculatorStrategy
    /// </summary>
    public class HammingDistanceCalculatorFactory
    {
        /// <summary>
        /// Creates an instance of IHammingDistanceCalculatorStrategy based on the strategy
        /// </summary>
        /// <param name="strategy">Strategy to be instantiated based on HammingDistanceStrategies enum</param>
        /// <returns>An instance of IHammingDistanceCalculatorStrategy</returns>
        public static IHammingDistanceCalculatorStrategy CreateInstance(HammingDistanceStrategies strategy)
        {
            switch (strategy)
            {
                case HammingDistanceStrategies.Parallel:
                    return new HammingDistanceCalculatorParallel();
                default:
                    return new HammingDistanceCalculatorStandard();
            }
        }
    }
}
