﻿using Google.OrTools.Sat;

public class SolutionPrinter(BoolVar[] xs, BoolVar[] ys) : CpSolverSolutionCallback
{
    private int SolutionCount_ = 0;
    private BoolVar[] Xs_ = xs;
    private BoolVar[] Ys_ = ys;

    public override void OnSolutionCallback()
    {
        SolutionCount_++;
        Console.WriteLine(String.Format("Solution #{0}: time = {1:F2} s", SolutionCount_, WallTime()));

        foreach (BoolVar x in Xs_)
        {
			Console.Write("{0} = {1} ", x.ToString(), Value(x));
        }

		Console.Write("  :   ");
		
        foreach (BoolVar y in Ys_)
        {
			Console.Write("{0} = {1} ", y.ToString(), Value(y));
        }

		Console.WriteLine();
    }

    public int SolutionCount()
    {
        return SolutionCount_;
    }
}

public class Problem
{
    static BoolVar BoolVarAnd(CpModel model, IEnumerable<ILiteral> literals, string? name = null)
    {
        if (name is null)
        {
			name = "";
        }

        BoolVar retval = model.NewBoolVar(name);
		int length = literals.Count();

        model.Add(LinearExpr.Sum(literals) + 1 <= retval + length);
        model.Add(length * retval <= LinearExpr.Sum(literals));

        return retval;
    }

    static BoolVar BoolVarOr(CpModel model, IEnumerable<ILiteral> literals, string? name = null)
    {
        if (name is null)
        {
			name = "";
        }

        BoolVar retval = model.NewBoolVar(name);
		int length = literals.Count();

		model.Add(LinearExpr.Sum(literals) <= length * retval);
		model.Add(retval <= LinearExpr.Sum(literals));

        return retval;
    }

    static int Main(string[] args)
    {
		if (args.Length != 1) {
			Console.Error.WriteLine($"Usage: dotnet run <InputFile>");
			return 1;
		}
		string inputFile = args[0];

		IEnumerable<string> lines = File.ReadLines(inputFile);
		
        CpModel model = new();

		BoolVar[] xs = new BoolVar[0];
		BoolVar[] ys = new BoolVar[0];
		int yCount = 0;

		foreach (string line in lines)
		{
			string[] lineArgs = line.Split(' ');

			if (lineArgs[0] == "c")
			{
				continue;
			}

			if (lineArgs[0] == "p")
			{
				if (lineArgs[1] != "cnf")
				{
					Console.Error.WriteLine($"Invalid param \"{lineArgs[1]}\" of line \"{line}\"");
					return 1;
				}

				int numVars = int.Parse(lineArgs[2]);
				int numTerms = int.Parse(lineArgs[3]);
				Console.WriteLine($"numVars = {numVars}, numTerms = {numTerms}");
				Console.WriteLine();
				Array.Resize(ref xs, numVars);
				Array.Resize(ref ys, numTerms);

				for (int i = 0; i < numVars; i++)
				{
					xs[i] = model.NewBoolVar($"x_{i + 1}");
				}

				continue;
			}

			ILiteral[] literals = new ILiteral[lineArgs.Length - 1];
			for (int i = 0; i < lineArgs.Length; i++)
			{
				int val = int.Parse(lineArgs[i]);
				if (val == 0)
				{
					break;
				}

				int varIdx = int.Abs(val) - 1;
				bool isNeg = val < 0;
				
				if (isNeg)
				{
					literals[i] = xs[varIdx].Not();
				}
				else
				{
					literals[i] = xs[varIdx];
				}
			}
			
			ys[yCount++] = BoolVarOr(model, literals, $"y_{yCount}");
			
			foreach (string arg in lineArgs)
			{
				Console.Write($"{arg } ");
			}
			Console.WriteLine();
		}
		Console.WriteLine();
		
		model.AddBoolAnd(ys);
		
        CpSolver solver = new();
        SolutionPrinter printer = new(xs, ys);
        solver.StringParameters = "enumerate_all_solutions:true";
        solver.Solve(model, printer);

        Console.WriteLine($"Number of solutions found: {printer.SolutionCount()}");

		return 0;
    }
}
