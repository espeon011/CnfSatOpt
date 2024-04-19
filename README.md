# CnfSatOpt
Solve sat problem of DIMACS CNF format

## Usage

```
$ dotnet run <InputFile>
```

## Example

```
$ dotnet run data/3sat.dimacs
numVars = 3, numTerms = 5

-1 -2 -3 0
1 -2 3 0
1 2 -3 0
1 -2 -3 0
-1 2 3 0

Solution #1: time = 0.00 s
x_1 = 0 x_2 = 0 x_3 = 0   :   y_1 = 1 y_2 = 1 y_3 = 1 y_4 = 1 y_5 = 1
Solution #2: time = 0.00 s
x_1 = 1 x_2 = 0 x_3 = 1   :   y_1 = 1 y_2 = 1 y_3 = 1 y_4 = 1 y_5 = 1
Solution #3: time = 0.00 s
x_1 = 1 x_2 = 1 x_3 = 0   :   y_1 = 1 y_2 = 1 y_3 = 1 y_4 = 1 y_5 = 1
Number of solutions found: 3  
```
