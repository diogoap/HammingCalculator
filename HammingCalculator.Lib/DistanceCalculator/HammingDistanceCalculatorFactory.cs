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
        /// <param name="strategy">Strategy to be instantiated based on HammingDistanceCalculatorStrategies enum</param>
        /// <returns>An instance of IHammingDistanceCalculatorStrategy</returns>
        public static IHammingDistanceCalculatorStrategy CreateInstance(HammingDistanceCalculatorStrategies strategy)
        {
            switch (strategy)
            {
                case HammingDistanceCalculatorStrategies.Parallel:
                    return new HammingDistanceCalculatorParallel();
                default:
                    return new HammingDistanceCalculatorStandard();
            }
        }
    }
}
