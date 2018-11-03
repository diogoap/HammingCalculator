namespace HammingCalculator.Lib.DistanceCalculator
{
    public interface IHammingDistanceCalculatorStrategy
    {
        int Calculate(byte[] input1, byte[] input2);
    }
}
