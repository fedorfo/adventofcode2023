namespace adventofcode2023;

using System.Text.RegularExpressions;
using helpers;

public class Day5 : PuzzleBase
{
    private static readonly Regex NumberRegex = new(@"Card\s+(\d+):\s+(.*)", RegexOptions.Compiled);

    public override void Solve()
    {
        var lines = ReadLines();
        lines.Add("");
        var seeds = Helpers.ExtractTokens(lines[0]).Skip(1).Select(long.Parse).ToList();
        lines = lines.Skip(2).ToList();
        var maps = new List<Map>();
        for (var i = 0; i < 7; i++)
        {
            var (newLines, map) = ReadMap(lines);
            maps.Add(map);
            lines = newLines;
        }

        var result1 = seeds.Select(x => Transform(maps, x, out var _)).Min();
        Console.WriteLine(result1);

        long result2 = 1000000000;
        for (var i = 0; i < seeds.Count; i += 2)
        {
            var start = seeds[i];
            var end = seeds[i] + seeds[i + 1] - 1;
            while (start <= end)
            {
                result2 = Math.Min(result2, Transform(maps, start, out var linearLenght));
                start += linearLenght;
            }
        }

        Console.WriteLine(result2);
    }

    private static long Transform(List<Map> maps, long source, out long linearLength)
    {
        linearLength = 1000000000;
        foreach (var map in maps)
        {
            source = map.Transform(source, out var linearLengthCandidate);
            linearLength = Math.Min(linearLength, linearLengthCandidate);
        }

        return source;
    }

    private static (List<string>, Map) ReadMap(List<string> lines)
    {
        var rules = new List<Rule>();
        int i;
        for (i = 1;; i++)
        {
            var tokens = Helpers.ExtractTokens(lines[i]).Select(long.Parse).ToList();
            if (tokens.Count == 0)
            {
                break;
            }

            rules.Add(new Rule(tokens[0], tokens[1], tokens[2]));
        }

        return (lines.Skip(i + 1).ToList(), new Map(rules));
    }

    private sealed record Rule(long DestinationStart, long SourceStart, long Length);

    private sealed record Map(List<Rule> Rules)
    {
        public long Transform(long source, out long linearLength)
        {
            foreach (var rule in this.Rules)
            {
                if (source >= rule.SourceStart && source <= rule.SourceStart + rule.Length - 1)
                {
                    linearLength = rule.SourceStart + rule.Length - source;
                    return source - rule.SourceStart + rule.DestinationStart;
                }
            }

            var biggerRules = this.Rules.Where(x => x.SourceStart > source).ToList();
            if (biggerRules.Count == 0)
            {
                linearLength = 1000000000;
            }
            else
            {
                linearLength = biggerRules.Select(x => x.SourceStart).Min() - source;
            }

            return source;
        }
    }
}
