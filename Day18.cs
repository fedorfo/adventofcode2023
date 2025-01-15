namespace adventofcode2023;

using helpers;

public class Day18 : PuzzleBase
{
    private readonly Dictionary<char, char> directionChar = new()
    {
        { '0', 'R' }, { '1', 'D' }, { '2', 'L' }, { '3', 'U' }
    };

    private readonly Dictionary<char, V2> directionVector = new()
    {
        { 'U', new V2(0, -1) }, { 'D', new V2(0, 1) }, { 'L', new V2(-1, 0) }, { 'R', new V2(1, 0) }
    };

    // public override string InputFileName => "sample.txt";

    private long SolveByVectors(List<V2> vectors)
    {
        var prev = new V2(0, 0);
        var points = new List<V2>();
        foreach (var vector in vectors)
        {
            var next = prev + vector;
            points.Add(next);
            prev = next;
        }

        var result = (long)0;
        for (var i = 2; i < points.Count; i++)
        {
            result += (points[i] - points[0]) * (points[i - 1] - points[0]);
        }

        result /= 2;
        result = Math.Abs(result);
        return result + (vectors.Select(x => x.ManhattanLength()).Sum() / 2) + 1;
    }

    public override void Solve()
    {
        var lines = ReadLines();
        var moves = lines
            .Select(x => Helpers.ExtractTokens(x, ' ', '(', ')', '#').ToArray())
            .Select(x => new Command(x[0][0], int.Parse(x[1]), x[2])).ToArray();
        Console.WriteLine(
            this.SolveByVectors(moves.Select(x => this.directionVector[x.Direction] * x.Distance).ToList()));
        Console.WriteLine(Convert.ToInt32("70c71", 16));
        Console.WriteLine(
            this.SolveByVectors(
                moves.Select(x =>
                        this.directionVector[this.directionChar[x.Color[5]]] *
                        Convert.ToInt32(x.Color.Substring(0, 5), 16))
                    .ToList()
            )
        );
    }


    private record Command(char Direction, int Distance, string Color);
}
