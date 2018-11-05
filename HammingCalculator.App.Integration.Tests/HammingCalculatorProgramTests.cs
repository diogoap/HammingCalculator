using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;

namespace HammingCalculator.App.Integration.Tests
{
    /// <summary>
    /// Integration tests for HammingCalculatorApp program.
    /// Tests will run against the assemblies contained at HammingCalculator.App program folder.
    /// </summary>
    [TestClass]
    public class HammingCalculatorProgramTests
    {
        private const string SourceInlineParam = "-inline";
        private const string SourceFileParam = "-file";
        private const string MethodParam = "-method";
        private const string DotNetCommand = "dotnet";
        private const string HammingCalculatorProgramName = "HammingCalculator.App.dll";
        private const string HammingCalculatorProgramDir = "HammingCalculator.App";
        private const string HammingCalculatorIntegrationTestsDir = "HammingCalculator.App.Integration.Tests";

        [TestMethod]
        [DataRow("0000", "0000", 0)]
        [DataRow("0011", "1100", 4)]
        [DataRow("0101010101", "0101010100", 1)]
        [DataRow("01010101010101010101", "01010101110101010100", 2)]
        public void ProgramShouldPrintExpectedDistanceInTheOutputWhenArgumentsAreValid(string input1, string input2, int expectedDistance)
        {
            // Arrange
            var expectedText = $"Hamming distance for the provided inputs is {expectedDistance}.";

            // Act
            var result = RunConsoleApplication(new string[] { HammingCalculatorProgramName, SourceInlineParam, input1, input2 });

            // Assert
            Assert.AreEqual(0, result.ExitCode);
            Assert.IsTrue(string.IsNullOrEmpty(result.Errors));
            Assert.IsTrue(result.Output.Contains(expectedText));
        }

        [TestMethod]
        public void ProgramShouldReturnErrorExitCodeAndInvalidArgumentMessageWhenSourceArgumentIsMissing()
        {
            // Arrange
            var expectedText = $"Either {SourceInlineParam} or {SourceFileParam} argument should be provided";

            // Act
            var result = RunConsoleApplication(new string[] { HammingCalculatorProgramName });

            // Assert
            Assert.AreNotEqual(0, result.ExitCode);
            Assert.IsTrue(result.Errors.Contains(expectedText));
            Assert.IsTrue(string.IsNullOrEmpty(result.Output));
        }

        [TestMethod]
        [DataRow(null, null)]
        [DataRow("1001", null)]
        [DataRow("", "")]
        public void ProgramShouldReturnErrorExitCodeAndInvalidArgumentMessageWhenArgumentsAreInvalid(string input1, string input2)
        {
            // Arrange
            var expectedText = $"Argument {SourceInlineParam} was not provided";

            // Act
            var result = RunConsoleApplication(new string[] { HammingCalculatorProgramName, SourceInlineParam, input1, input2 });

            // Assert
            Assert.AreNotEqual(0, result.ExitCode);
            Assert.IsTrue(result.Errors.Contains(expectedText));
            Assert.IsTrue(string.IsNullOrEmpty(result.Output));
        }

        [TestMethod]
        [DataRow("0101", "010101")]
        public void ProgramShouldReturnErrorExitCodeAndInputLengthErrorWhenArgumentsLegnthAreDifferent(string input1, string input2)
        {
            // Arrange
            var expectedText = "Inputs must be equal length";

            // Act
            var result = RunConsoleApplication(new string[] { HammingCalculatorProgramName, SourceInlineParam, input1, input2 });

            // Assert
            Assert.AreNotEqual(0, result.ExitCode);
            Assert.IsTrue(result.Errors.Contains(expectedText));
            Assert.IsTrue(string.IsNullOrEmpty(result.Output));
        }

        [TestMethod]
        [DataRow("0101", "0101", "", "", "Strategy: HammingCalculator.Lib.DistanceCalculator.HammingDistanceCalculatorStandard")]
        [DataRow("0101", "0101", MethodParam, "Standard", "Strategy: HammingCalculator.Lib.DistanceCalculator.HammingDistanceCalculatorStandard")]
        [DataRow("0101", "0101", MethodParam, "Parallel", "Strategy: HammingCalculator.Lib.DistanceCalculator.HammingDistanceCalculatorParallel")]
        [DataRow("0101", "0101", MethodParam, "Unknown", "Strategy: HammingCalculator.Lib.DistanceCalculator.HammingDistanceCalculatorStandard")]
        public void ProgramShouldUseCorrespondStrategyAccordingToTheMethodArgument(string input1, string input2, string methodParam, string strategy, string expectedText)
        {
            // Act
            var result = RunConsoleApplication(new string[] { HammingCalculatorProgramName, SourceInlineParam,
                input1, input2, methodParam, strategy });

            // Assert
            Assert.AreEqual(0, result.ExitCode);
            Assert.IsTrue(result.Output.Contains(expectedText));
        }

        [TestMethod]
        public void ProgramShouldReturnErrorExitCodeAndFileNotFoundErrorWhenFile1IsNotFound()
        {
            // Arrange
            var fileName1 = "Z:\\InvalidRootFolder\\InvalidFolder\\InvalidFile1.txt";
            var expectedResult = $"File1 {fileName1} cannot be found";

            var fileName2 = Path.GetTempFileName();
            File.WriteAllBytes(fileName2, new byte[100]);

            // Act
            var result = RunConsoleApplication(new string[] { HammingCalculatorProgramName, SourceFileParam, fileName1, fileName2 });

            // Assert
            Assert.AreNotEqual(0, result.ExitCode);
            Assert.IsTrue(result.Errors.Contains(expectedResult));

            // Removing temp files
            File.Delete(fileName2);
        }

        [TestMethod]
        public void ProgramShouldReturnErrorExitCodeAndFileNotFoundErrorWhenFile2IsNotFound()
        {
            // Arrange
            var fileName1 = Path.GetTempFileName();
            File.WriteAllBytes(fileName1, new byte[100]);

            var fileName2 = "Z:\\InvalidFolder\\InvalidFile2.txt";
            var expectedResult = $"File2 {fileName2} cannot be found";

            // Act
            var result = RunConsoleApplication(new string[] { HammingCalculatorProgramName, SourceFileParam, fileName1, fileName2 });

            // Assert
            Assert.AreNotEqual(0, result.ExitCode);
            Assert.IsTrue(result.Errors.Contains(expectedResult));

            // Removing temp files
            File.Delete(fileName1);
        }

        [TestMethod]
        public void ProgramShouldProcessFilesWhenSourceFilesIsUsedAndUseStandardStrategyWhenMethodIsNotProvided()
        {
            // Arrange
            var fileName1 = Path.GetTempFileName();
            var fileName2 = Path.GetTempFileName();

            File.WriteAllBytes(fileName1, new byte[100]);
            File.WriteAllBytes(fileName2, new byte[100]);

            var expectedResult = "Hamming distance for the provided inputs is 0";
            var expectedMethod = "Strategy: HammingCalculator.Lib.DistanceCalculator.HammingDistanceCalculatorStandard";

            // Act
            var result = RunConsoleApplication(new string[] { HammingCalculatorProgramName, SourceFileParam, fileName1, fileName2 });

            // Assert
            Assert.AreEqual(0, result.ExitCode);
            Assert.IsTrue(result.Output.Contains(expectedResult));
            Assert.IsTrue(result.Output.Contains(expectedMethod));

            // Removing temp files
            File.Delete(fileName1);
            File.Delete(fileName2);
        }

        [TestMethod]
        public void ProgramShouldUseParallelStrategyAndProcessFilesWhenSourceFilesIsUsedAndMethodIsParallel()
        {
            // Arrange
            var fileName1 = Path.GetTempFileName();
            var fileName2 = Path.GetTempFileName();

            File.WriteAllBytes(fileName1, new byte[100]);
            File.WriteAllBytes(fileName2, new byte[100]);

            var expectedResult = "Hamming distance for the provided inputs is 0";
            var expectedMethod = "Strategy: HammingCalculator.Lib.DistanceCalculator.HammingDistanceCalculatorParallel";

            // Act
            var result = RunConsoleApplication(new string[] { HammingCalculatorProgramName, SourceFileParam,
                fileName1, fileName2, MethodParam, "Parallel" });

            // Assert
            Assert.AreEqual(0, result.ExitCode);
            Assert.IsTrue(result.Output.Contains(expectedResult));
            Assert.IsTrue(result.Output.Contains(expectedMethod));

            // Removing temp files
            File.Delete(fileName1);
            File.Delete(fileName2);
        }

        [TestMethod]
        public void ProgramShouldCalculateExpectedDistanceAndUseParallelStrategyWhenSourceFilesIsUsedAndInALargeFiles()
        {
            // Arrange
            var fileName1 = Path.GetTempFileName();
            var fileName2 = Path.GetTempFileName();

            var input1 = new byte[100_000_000];
            // Inverting some bit's values...
            input1[0] ^= 1;
            input1[50] ^= 1;
            input1[5_000] ^= 1;
            input1[500_000] ^= 1;
            input1[99_000_000] ^= 1;

            File.WriteAllBytes(fileName1, input1);
            File.WriteAllBytes(fileName2, new byte[100_000_000]);

            var expectedResult = "Hamming distance for the provided inputs is 5";
            var expectedMethod = "Strategy: HammingCalculator.Lib.DistanceCalculator.HammingDistanceCalculatorParallel";

            // Act
            var result = RunConsoleApplication(new string[] { HammingCalculatorProgramName, SourceFileParam,
                fileName1, fileName2, MethodParam, "Parallel" });

            // Assert
            Assert.AreEqual(0, result.ExitCode);
            Assert.IsTrue(result.Output.Contains(expectedResult));
            Assert.IsTrue(result.Output.Contains(expectedMethod));

            // Removing temp files
            File.Delete(fileName1);
            File.Delete(fileName2);
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

            // Switches working directory from current project (IntegrationTests) to console app folder (HammingCalculator.App)
            var workingDirectory = Environment.CurrentDirectory.Replace(HammingCalculatorIntegrationTestsDir, HammingCalculatorProgramDir);
            proc.StartInfo.WorkingDirectory = workingDirectory;

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
