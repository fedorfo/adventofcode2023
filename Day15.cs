namespace adventofcode2023;

using System.Globalization;
using helpers;

public class Day15 : PuzzleBase
{
    public override void Solve()
    {
        var tokens = Helpers.ExtractTokens(ReadLines()[0], ',').ToList();
        var result1 = tokens.Select(Hash).Sum();
        Console.WriteLine(result1);

        var boxes = Enumerable.Range(0, 256).Select(_ => new List<Lens>()).ToList();
        foreach (var token in tokens)
        {
            if (token.EndsWith('-'))
            {
                var lensLabel = token.Substring(0, token.Length - 1);
                var lensHash = Hash(lensLabel);
                boxes[lensHash] = boxes[lensHash].Where(x => x.Label != lensLabel).ToList();
            }
            else if (token.Contains('='))
            {
                var lensTokens = Helpers.ExtractTokens(token, '=').ToList();
                var lens = new Lens(lensTokens[0], int.Parse(lensTokens[1], CultureInfo.InvariantCulture));
                var lensHash = Hash(lens.Label);
                var existingLens = boxes[lensHash].SingleOrDefault(x => x.Label == lens.Label);
                if (existingLens != null)
                {
                    existingLens.FocalLength = lens.FocalLength;
                }
                else
                {
                    boxes[lensHash].Add(lens);
                }
            }
        }

        var result2 = boxes
            .Select(
                (box, boxIndex) => box
                    .Select((lens, lensIndex) => (lensIndex + 1) * (boxIndex + 1) * lens.FocalLength)
                    .Sum()
            )
            .Sum();
        Console.WriteLine(result2);
    }

    private static int Hash(string text)
    {
        var result = 0;
        foreach (var symbol in text)
        {
            result += symbol;
            result *= 17;
            result &= 255;
        }

        return result;
    }

    private sealed record Lens(string Label, int FocalLength)
    {
        public int FocalLength { get; set; } = FocalLength;
    }
}
