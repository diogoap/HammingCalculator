using HammingCalculator.Lib.DistanceCalculator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace HammingCalculator.Lib.Tests
{
    /// <summary>
    /// Units tests for HammingCalculatorHelperTests class.
    /// </summary>
    [TestClass]
    public class HammingCalculatorHelperTests
    {
        private HammingCalculatorHelper _hammingCalculatorHelper;
        private Mock<IHammingDistanceCalculatorStrategy> _hammingDistanceCalculatorStrategyMock;

        [TestInitialize]
        public void Initialize()
        {
            _hammingDistanceCalculatorStrategyMock = new Mock<IHammingDistanceCalculatorStrategy>();
            _hammingCalculatorHelper = new HammingCalculatorHelper(_hammingDistanceCalculatorStrategyMock.Object);
        }

        [TestMethod]
        public void CalculateDistanceShouldCallCalculateMethodOnceWhenInputsAreValid()
        {
            // Arrange
            var input1 = new byte[] { 0, 0, 1 };
            var input2 = new byte[] { 0, 1, 1 };

            // Act
            var result = _hammingCalculatorHelper.CalculateDistance(input1, input2);

            // Assert
            _hammingDistanceCalculatorStrategyMock.Verify(m => m.Calculate(It.IsAny<byte[]>(), It.IsAny<byte[]>()), Times.Once); 
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CalculateDistanceShouldThrowExceptionWhenInputsAreNotEqualLength()
        {
            // Arrange
            var input1 = new byte[] { 0, 0, 1 };
            var input2 = new byte[] { 0, 0, 1, 0 };

            // Act
            var result = _hammingCalculatorHelper.CalculateDistance(input1, input2);
        }

        [TestMethod]
        [DataRow(null, null)]
        [DataRow(new byte[] { 0, 0 }, null)]
        [DataRow(null, new byte[] { 0, 0 })]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CalculateDistanceShouldThrowExceptionWhenInputsAreNotProvided(byte[] input1, byte[] input2)
        {
            // Act
            var result = _hammingCalculatorHelper.CalculateDistance(input1, input2);
        }
    }
}
