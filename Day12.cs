namespace adventofcode2023;

using helpers;

public class Day12 : PuzzleBase
{
    public override void Solve()
    {
        var springs = ReadLines().Select(x =>
        {
            var tokens = Helpers.ExtractTokens(x).ToArray();
            return new Spring(tokens[0], Helpers.ExtractTokens(tokens[1], ',').Select(int.Parse).ToList());
        }).ToList();

        var result1 = springs.Select(x => Solve(x, 0, 0, new Dictionary<Tuple<int, int>, long>())).Sum();
        Console.WriteLine(result1);

        springs = springs.Select(x => new Spring
            (
                string.Join('?', Enumerable.Repeat(x.Pattern, 5)),
                Enumerable.Repeat(x.DamagedParts, 5).SelectMany(y => y).ToList()
            )
        ).ToList();

        var result2 = springs.Select(x => Solve(x, 0, 0, new Dictionary<Tuple<int, int>, long>())).Sum();
        Console.WriteLine(result2);
    }

    private static long Solve(Spring spring, int patternIndex, int damagedPartsIndex,
        Dictionary<Tuple<int, int>, long> hash)
    {
        var key = Tuple.Create(patternIndex, damagedPartsIndex);
        if (hash.TryGetValue(key, out var hashedResult))
        {
            return hashedResult;
        }

        if (damagedPartsIndex >= spring.DamagedParts.Count)
        {
            return spring.Pattern.Skip(patternIndex).All(".?".Contains) ? 1 : 0;
        }

        if (patternIndex >= spring.Pattern.Length)
        {
            return 0;
        }

        long result = 0;
        if ("#?".Contains(spring.Pattern[patternIndex]))
        {
            var candidate = spring.Pattern.Skip(patternIndex).Take(spring.DamagedParts[damagedPartsIndex]).ToList();
            if (
                candidate.Count == spring.DamagedParts[damagedPartsIndex] &&
                candidate.All("#?".Contains) &&
                (
                    patternIndex + spring.DamagedParts[damagedPartsIndex] >= spring.Pattern.Length ||
                    "?.".Contains(spring.Pattern[patternIndex + spring.DamagedParts[damagedPartsIndex]])
                )
            )
            {
                result += Solve(spring, patternIndex + spring.DamagedParts[damagedPartsIndex] + 1,
                    damagedPartsIndex + 1, hash);
            }
        }

        if (".?".Contains(spring.Pattern[patternIndex]))
        {
            result += Solve(spring, patternIndex + 1, damagedPartsIndex, hash);
        }

        hash[key] = result;
        return result;
    }

    private sealed record Spring(string Pattern, List<int> DamagedParts);
}
