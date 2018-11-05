[![Build Status](https://travis-ci.org/diogoap82/HammingCalculator.svg?branch=master)](https://travis-ci.org/diogoap82/HammingCalculator)

# Hamming Calculator

This program calculates the Hamming Distance for 2 binary inputs.

Solution is written in C# targeting .NET Core 2.1. Source code is integrated with TravisCI to allow Continuous Integration.

The program expects the following arguments:

1. Data source: argument name must be either **-inline** or **-file**
	 - 2 values are required
	 - If source is **-inline**:
		 - Values must be two strings containing a sequence of bits. Example: "0101010101010101".
		 - Strings must be equal size.
	 - If source is **-file**:
		 - Values must be two valid file names. Example: "C:\file.txt".
		 - File contents must be equal size.
            
2. Method of calculation: argument name is **-method**
	 - Optional
	 - Value should be:
		 - Standard: to select default calculation method.
		 - Parallel: to perform the calculation with parallelism. Should be used in large amounts of data.

## Usage instructions

To call the program, specify the data source and provide two strings containing either inline data or the file names where data will be read from. For example:

```bat
dotnet HammingCalculator.App.dll -inline 0011 1101
```
```bat
dotnet HammingCalculator.App.dll -file "C:\temp\file1.txt" "C:\temp\file2.txt"
```
Also, the calculation method can be specified. For example:
```bat
dotnet HammingCalculator.App.dll -file "C:\temp\file1.txt" "C:\temp\file2.txt" -method Parallel
```
```bat
dotnet HammingCalculator.App.dll -inline 0011 1101 -method Standard
```
If **-method** argument is not provided, Standard method is considered.

## Dealing with large amounts of data

The program provides an alternative approach to calculate the Hamming Distance for large amounts of data. When used, this approach split the data into small chunks and process each part simultaneously through .Net Parallel library.

Using Parallel, the calculation is executed about 75% faster than the regular method, or even more. 
Here is a comparison of two calculation methods, processing files with 91,8 MB. First, let's use the Standard method:

```bat
dotnet HammingCalculator.App.dll -file "C:\temp\file1.txt" "C:\temp\file2.txt" -method Standard
Calculating Hamming Distance for the inputs below:
Input 1: C:\temp\file1.txt
Input 2: C:\temp\file2.txt
Strategy: HammingCalculator.Lib.DistanceCalculator.HammingDistanceCalculatorStandard

Hamming distance for the provided inputs is 5.

Elapsed Milliseconds: 410
```

Now the Parallel execution method:
```bat

dotnet HammingCalculator.App.dll -file "C:\temp\file1.txt" "C:\temp\file2.txt" -method Parallel
Calculating Hamming Distance for the inputs below:
Input 1: C:\temp\file1.txt
Input 2: C:\temp\file2.txt
Strategy: HammingCalculator.Lib.DistanceCalculator.HammingDistanceCalculatorParallel

Hamming distance for the provided inputs is 5.

Elapsed Milliseconds: 216
```

Standard method took 410 ms and Parallel method ran in 216 ms.

## Improvements

Here are some suggestions for future improvements: 
- Usage of a DI Container to instantiate an IHammingDistanceCalculatorStrategy class instead of using a factory. This change would make the maintenance simpler.
- Adopt a 3rd library to parse the program arguments. Currently, ProgramOptions class is responsible for this task. However, using some 3rd library would bring more capabilities and remove this concern of the application.
- Expand CI capabilities by adding an automatic deployment into CI pipeline.