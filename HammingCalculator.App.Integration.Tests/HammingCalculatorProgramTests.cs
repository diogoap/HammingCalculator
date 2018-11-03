using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;

namespace HammingCalculator.App.Integration.Tests
{
    /// <summary>
    /// Integration tests for HammingCalculatorApp program.
    /// Tests will run against the assemblies contained at Program folder located at solution root folder.
    /// </summary>
    [TestClass]
    public class HammingCalculatorProgramTests
    {
        private const string DotNetCommand = "dotnet";
        private const string HammingCalculatorProgram = "HammingCalculator.App.dll";
        private const string HammingCalculatorProgramPath = "Program";
        private const string IntegrationTestsDir = "HammingCalculator.App.Integration.Tests";

        [TestMethod]
        [DataRow("0000", "0000", 0)]
        [DataRow("0011", "1100", 4)]
        [DataRow("0101010101", "0101010100", 1)]
        [DataRow("01010101010101010101", "01010101110101010100", 2)]
        public void ProgramShouldPrintExpectedDistanceInTheOutput(string input1, string input2, int expectedDistance)
        {
            // Arrange
            var expectedText = $"Hamming distance for the provided inputs is {expectedDistance}.";

            // Act
            var result = RunConsoleApplication(new string[] { HammingCalculatorProgram, input1, input2 });

            // Assert
            Assert.AreEqual(0, result.ExitCode);
            Assert.IsTrue(string.IsNullOrEmpty(result.Errors));
            Assert.IsTrue(result.Output.Contains(expectedText));
        }

        [TestMethod]
        [DataRow(null, null)]
        [DataRow("1001", null)]
        [DataRow("", "")]
        public void ProgramShouldReturnErrorExitCodeAndInvalidArgumentMessageWhenArgumentsAreInvalid(string input1, string input2)
        {
            // Arrange
            var expectedText = "Invalid arguments";

            // Act
            var result = RunConsoleApplication(new string[] { HammingCalculatorProgram, input1, input2 });

            // Assert
            Assert.AreNotEqual(0, result.ExitCode);
            Assert.IsTrue(result.Errors.Contains(expectedText));
            Assert.IsTrue(string.IsNullOrEmpty(result.Output));
        }

        [TestMethod]
        [DataRow("0101", "010101")]
        public void ProgramShouldReturnErrorExitCodeAndInputLengthErrorWhenArgumentLegnthAreDifferent(string input1, string input2)
        {
            // Arrange
            var expectedText = "Inputs must be equal length";

            // Act
            var result = RunConsoleApplication(new string[] { HammingCalculatorProgram, input1, input2 });

            // Assert
            Assert.AreNotEqual(0, result.ExitCode);
            Assert.IsTrue(result.Errors.Contains(expectedText));
            Assert.IsTrue(string.IsNullOrEmpty(result.Output));
        }

        [TestMethod]
        [DataRow("0101", "0101", "", "Strategy: HammingCalculator.Lib.DistanceCalculator.HammingDistanceCalculatorStandard")]
        [DataRow("0101", "0101", "0", "Strategy: HammingCalculator.Lib.DistanceCalculator.HammingDistanceCalculatorStandard")]
        [DataRow("0101", "0101", "1", "Strategy: HammingCalculator.Lib.DistanceCalculator.HammingDistanceCalculatorParallel")]
        [DataRow("0101", "0101", "999", "Strategy: HammingCalculator.Lib.DistanceCalculator.HammingDistanceCalculatorStandard")]
        public void ProgramShouldUseCorrespondStrategyAccordingToTheArguments(string input1, string input2, string strategy, string expectedText)
        {
            // Act
            var result = RunConsoleApplication(new string[] { HammingCalculatorProgram, input1, input2, strategy });

            // Assert
            Assert.AreEqual(0, result.ExitCode);
            Assert.IsTrue(result.Output.Contains(expectedText));
        }

        /// <summary>
        /// Runs a console application with the provided arguments and returns the results
        /// </summary>
        /// <param name="arguments">Arguments which will be sent to the console application</param>
        /// <returns>An object with the output messages, errors and exit code</returns>
        private HammingCalculatorProcessResult RunConsoleApplication(string[] arguments)
        {
            // Initialize process
            Process proc = new Process();
            proc.StartInfo.FileName = DotNetCommand;

            // Build arguments list
            proc.StartInfo.Arguments = string.Join(" ", arguments);

            // Redirect outputs and errors in order to capture them
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;

            // Set working directory to {Solution root folder}\{Program folder}
            var workingDirectory = Environment.CurrentDirectory;
            workingDirectory = Environment.CurrentDirectory.Substring(0, workingDirectory.IndexOf(IntegrationTestsDir));
            proc.StartInfo.WorkingDirectory = Path.Join(workingDirectory, HammingCalculatorProgramPath);

            // Start and wait for exit
            proc.Start();
            proc.WaitForExit();

            // Get output messages, errors and exit code
            return new HammingCalculatorProcessResult
            {
                ExitCode = proc.ExitCode,
                Output = proc.StandardOutput.ReadToEnd(),
                Errors = proc.StandardError.ReadToEnd()
            };
        }
    }
}
