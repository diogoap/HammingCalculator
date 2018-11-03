namespace HammingCalculator.App.Integration.Tests
{
    /// <summary>
    /// Represents the results of HammingCalculator process execution.
    /// </summary>
    public class HammingCalculatorProcessResult
    {
        public int ExitCode { get; set; }
        public string Output { get; set; }
        public string Errors { get; set; }
    }
}
