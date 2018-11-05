[![Build Status](https://travis-ci.org/diogoap82/HammingCalculator.svg?branch=master)](https://travis-ci.org/diogoap82/HammingCalculator)

# HammingCalculator

This program calculates the Hamming Distance for 2 binary inputs.

Arguments expected:

1. Data source: argument name must be either **-inline** or **-file**
	 - 2 values are required
	 - If source is **-inline**:
		 - Values must be two strings containing a sequence of bits. Example: "0101010101010101".
		 - Strings must be equal size.
	 - If source is **-file**:
		 - Values must be two valid file names. Example: "C:\file.txt".
		 - File contents must be equal size.
            
2. Method of caculation: argument name is **-method**
	 - Optional
	 - Value should be:
		 - Standard: to select default calculation method.
		 - Parallel: to perform the calculation with paralelism. Should be used in large amounts of data.

## Usage instructons

To call the program, specify the data source and provide two strings containg either inline data or file names where data will be read from. For example:

```bat
dotnet HammingCalculator.App.dll -inline 0011 1101
```
```bat
dotnet HammingCalculator.App.dll -file "C:\temp\file1.txt" "C:\temp\file2.txt"
```
Also, the calculation method can be specified. For exemple:
```bat
dotnet HammingCalculator.App.dll -file "C:\temp\file1.txt" "C:\temp\file2.txt" -method Parallel
```
```bat
dotnet HammingCalculator.App.dll -inline 0011 1101 -method Standard
```
If **-method** argument is not provided, Standard method is considered.

## Dealing with large amounts of data

The program provides an alternative approach to calculate the Hamming Distance for large amounts of data. When used, this approach split the data into small chunks and process each part simultaneously through .Net Parallel library.

Using Parallel, the calculation is executed about 75% faster than the regular method. 
Here is a comparison of two calculation methods in 

```bat
dotnet HammingCalculator.App.dll -file "C:\temp\file1.txt" "C:\temp\file2.txt" -method Standard
Calculating Hamming Distance for the inputs below:
Input 1: C:\temp\file1.txt
Input 2: C:\temp\file2.txt
Strategy: HammingCalculator.Lib.DistanceCalculator.HammingDistanceCalculatorStandard

Hamming distance for the provided inputs is 5.

Elapsed Milliseconds: 410
```
Now the Parallel execution:
```bat

dotnet HammingCalculator.App.dll -file "C:\temp\file1.txt" "C:\temp\file2.txt" -method Parallel
Calculating Hamming Distance for the inputs below:
Input 1: C:\temp\file1.txt
Input 2: C:\temp\file2.txt
Strategy: HammingCalculator.Lib.DistanceCalculator.HammingDistanceCalculatorParallel

Hamming distance for the provided inputs is 5.

Elapsed Milliseconds: 216
```

## Improvements

As a suggestions for future improvemnts 
Container DI to instantiate the strategy