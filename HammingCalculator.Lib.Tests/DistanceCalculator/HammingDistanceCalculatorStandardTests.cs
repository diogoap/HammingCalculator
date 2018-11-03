using HammingCalculator.Lib.DistanceCalculator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HammingCalculator.Lib.Tests
{
    /// <summary>
    /// Units tests for HammingDistanceCalculatorStandard class.
    /// </summary>
    [TestClass]
    public class HammingDistanceCalculatorStandardTests
    {
        private HammingDistanceCalculatorStandard _hammingDistanceCalculator;

        [TestInitialize]
        public void Initialize()
        {
            _hammingDistanceCalculator = new HammingDistanceCalculatorStandard();
        }

        [TestMethod]
        [DataRow(new byte[] { 0, 1, 1, 0, 0 }, new byte[] { 0, 1, 1, 0, 0 }, 0)]
        [DataRow(new byte[] { 0, 1, 1, 0, 0 }, new byte[] { 0, 0, 1, 0, 0 }, 1)]
        [DataRow(new byte[] { 0, 1, 1, 0, 0 }, new byte[] { 0, 0, 1, 1, 0 }, 2)]
        [DataRow(new byte[] { 0, 1, 1, 0, 0 }, new byte[] { 1, 0, 1, 0, 1 }, 3)]
        public void CalculateDistanceShouldReturnExpectedDistance(byte[] input1, byte[] input2, int expectedDistance)
        {
            // Act
            var result = _hammingDistanceCalculator.Calculate(input1, input2);

            // Assert
            Assert.AreEqual(expectedDistance, result);
        }

        [TestMethod]
        public void CalculateDistanceShouldReturnDistanceTenWhenInputsHaveTenDifferencesInLargeInput()
        {
            // Arrange
            var input1 = new byte[1_000_000_000];
            var input2 = new byte[1_000_000_000];

            // Inverting some bit's value...
            input1[5] ^= 1;
            input1[50] ^= 1;
            input1[500] ^= 1;
            input1[5_000] ^= 1;
            input1[50_000] ^= 1;
            input1[500_000] ^= 1;
            input1[5_000_000] ^= 1;
            input1[50_000_000] ^= 1;
            input1[500_000_000] ^= 1;
            input1[999_999_999] ^= 1;

            // Act
            var result = _hammingDistanceCalculator.Calculate(input1, input2);

            // Assert
            Assert.AreEqual(10, result);
        }
    }
}
