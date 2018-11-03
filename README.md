[![Build Status](https://travis-ci.org/diogoap82/HammingCalculator.svg?branch=master)](https://travis-ci.org/diogoap82/HammingCalculator)

# HammingCalculator

Calculates the Hamming Distance for 2 binary inputs.

To call the program provide two input strings with binary data. For example:

```bat
dotnet HammingCalculator.App.dll 0011 1101
```

Program will then return the results:

```
Calculating Hamming Distance for the inputs below:
Input 1: 0011
Input 2: 1101
Strategy: HammingCalculator.Lib.DistanceCalculator.HammingDistanceCalculatorStandard

Hamming distance for the provided inputs is 3.
```

```bat
dotnet HammingCalculator.App.dll 0011 1101 1
```