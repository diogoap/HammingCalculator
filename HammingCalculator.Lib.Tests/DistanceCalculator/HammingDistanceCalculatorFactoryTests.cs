using HammingCalculator.Lib.DistanceCalculator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace HammingCalculator.Lib.Tests
{
    /// <summary>
    /// Units tests for HammingDistanceCalculatorFactory class.
    /// </summary>
    [TestClass]
    public class HammingDistanceCalculatorFactoryTests
    {
        private HammingDistanceCalculatorFactory _hammingDistanceCalculatorFactory;

        [TestInitialize]
        public void Initialize()
        {
            _hammingDistanceCalculatorFactory = new HammingDistanceCalculatorFactory();
        }

        [TestMethod]
        [DataRow(HammingDistanceCalculatorStrategies.Standard, typeof(HammingDistanceCalculatorStandard))]
        [DataRow(HammingDistanceCalculatorStrategies.Parallel, typeof(HammingDistanceCalculatorParallel))]
        public void CreateInstanceShouldReturnInstanceOfExpectedType(HammingDistanceCalculatorStrategies strategy, Type expectedType)
        {
            // Act
            var result = HammingDistanceCalculatorFactory.CreateInstance(strategy);

            // Assert
            Assert.IsInstanceOfType(result, expectedType);
        }
    }
}
