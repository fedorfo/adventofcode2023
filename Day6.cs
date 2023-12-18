namespace adventofcode2023;

using System.Globalization;
using System.Text.RegularExpressions;
using helpers;

public class Day6 : PuzzleBase
{
    private static readonly Regex NumberRegex = new(@"Card\s+(\d+):\s+(.*)", RegexOptions.Compiled);

    public override void Solve()
    {
        var lines = ReadLines();
        var times = Helpers.ExtractTokens(lines[0]).Skip(1).Select(int.Parse).ToList();
        var distances = Helpers.ExtractTokens(lines[1]).Skip(1).Select(int.Parse).ToList();

        var result1 = 1;
        for (var i = 0; i < times.Count; i++)
        {
            var raceOptions = 0;
            for (var j = 0; j <= times[i]; j++)
            {
                if ((times[i] - j) * j > distances[i])
                {
                    raceOptions++;
                }
            }

            result1 *= raceOptions;
        }

        Console.WriteLine(result1);


        var time = long.Parse(string.Join("", Helpers.ExtractTokens(lines[0]).Skip(1)), CultureInfo.InvariantCulture);
        var distance = long.Parse(string.Join("", Helpers.ExtractTokens(lines[1]).Skip(1)),
            CultureInfo.InvariantCulture);
        var result2 = 0;
        for (long i = 0; i <= time; i++)
        {
            if ((time - i) * i >= distance)
            {
                result2++;
            }
        }

        Console.WriteLine(result2);
    }
}
