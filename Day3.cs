using System.Text.RegularExpressions;
using adventofcode2023.helpers;

namespace adventofcode2023;

public class Day3 : PuzzleBase
{
    private static readonly Regex NumberRegex = new(@"(\d+)", RegexOptions.Compiled);

    private record Number(int Value, int X, int Y)
    {
        public List<V2> Points => Enumerable.Range(Y, Value.ToString().Length).Select(d => new V2(X, d)).ToList();
    };

    public override void Solve()
    {
        var lines = ReadLines();
        var numbers = new List<Number>();
        for (var i = 0; i < lines.Count; i++)
        {
            var line = lines[i];
            var matches = NumberRegex.Matches(line).ToArray();
            foreach (var match in matches)
            {
                numbers.Add(new Number(int.Parse(match.Groups[1].Value), i, match.Groups[1].Index));
            }
        }

        var result1 = numbers.Select(number =>
        {
            var numberArea = number.Points
                .SelectMany(x => x.GetNeighbours8())
                .Where(x => x >= new V2(0, 0) && x < new V2(lines.Count, lines[0].Length));
            return numberArea.Any(x => !char.IsDigit(lines[x.X][x.Y]) && lines[x.X][x.Y] != '.') ? number.Value : 0;
        }).Sum();
        Console.WriteLine(result1);

        var pointToNumber = numbers
            .SelectMany(num => num.Points.Select(p => KeyValuePair.Create(p, num)))
            .ToDictionary(x => x.Key, x => x.Value);
        var gears = Enumerable.Range(0, lines.Count)
            .SelectMany(x => Enumerable.Range(0, lines[0].Length).Select(y => new V2(x, y)))
            .Where(p => lines[p.X][p.Y] == '*')
            .ToArray();

        var result2 = gears.Select(gear =>
        {
            var adjacentNumbers = new V2(gear.X, gear.Y)
                .GetNeighbours8()
                .Where(pointToNumber.ContainsKey)
                .Select(x => pointToNumber[x])
                .Distinct()
                .ToList();
            return adjacentNumbers.Count != 2 ? 0 : adjacentNumbers[0].Value * adjacentNumbers[1].Value;
        }).Sum();
        Console.WriteLine(result2);
    }
}