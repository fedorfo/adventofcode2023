namespace adventofcode2023;

using helpers;

public class Day9 : PuzzleBase
{
    public override void Solve()
    {
        var input = ReadLines().Select(x => Helpers.ExtractTokens(x).Select(int.Parse).ToList()).ToList();
        Console.WriteLine(input.Select(Solve1).Sum());
        Console.WriteLine(input.Select(Solve2).Sum());
    }

    private static int Solve1(List<int> data)
    {
        if (data.All(x => x == 0))
        {
            return 0;
        }

        var childSolution = Solve1(data.Skip(1).Select((_, i) => data[i + 1] - data[i]).ToList());
        return data.Last() + childSolution;
    }

    private static int Solve2(List<int> data)
    {
        if (data.All(x => x == 0))
        {
            return 0;
        }

        var childSolution = Solve2(data.Skip(1).Select((_, i) => data[i + 1] - data[i]).ToList());
        return data.First() - childSolution;
    }
}
