# CnfSatOpt
Solve sat problem of DIMACS CNF format

## Usage

```
$ dotnet run <InputFile>
```

## Example

```shell
$ cat data/3sat.dimacs
c example DIMACS-CNF 3-SAT
p cnf 3 5
-1 -2 -3 0
1 -2 3 0
1 2 -3 0
1 -2 -3 0
-1 2 3 0
```

```shell
$ dotnet run data/3sat.dimacs
numVars = 3, numTerms = 5

y_1 = !x_1 || !x_2 || !x_3
y_2 =  x_1 || !x_2 ||  x_3
y_3 =  x_1 ||  x_2 || !x_3
y_4 =  x_1 || !x_2 || !x_3
y_5 = !x_1 ||  x_2 ||  x_3
y_1 && y_2 && y_3 && y_4 && y_5 == true

Solution #1: time = 0.00 s
x_1 = 0 x_2 = 0 x_3 = 0
Solution #2: time = 0.00 s
x_1 = 1 x_2 = 0 x_3 = 1
Solution #3: time = 0.00 s
x_1 = 1 x_2 = 1 x_3 = 0
Number of solutions found: 3
```
