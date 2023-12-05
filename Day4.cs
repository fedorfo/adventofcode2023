namespace adventofcode2023;

using System.Text.RegularExpressions;
using helpers;

public class Day4 : PuzzleBase
{
    private static readonly Regex NumberRegex = new(@"Card\s+(\d+):\s+(.*)", RegexOptions.Compiled);

    public override void Solve()
    {
        var lines = ReadLines();
        var cards = lines.Select(line =>
        {
            var match = NumberRegex.Match(line);
            var groups = match.Groups[2].Value.Split(" | ");
            return new Card(
                groups[0].Split(" ").Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse).ToList(),
                groups[1].Split(" ").Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse).ToList()
            );
        }).ToList();
        var result1 = cards.Select(card =>
        {
            var pow = card.WinsCount - 1;
            return pow == -1 ? 0 : Helpers.LongPow(2, pow);
        }).Sum();
        Console.WriteLine(result1);

        var numberOfCards = Enumerable.Range(0, cards.Count).Select(_ => 1).ToList();
        for (var i = 0; i < cards.Count; i++)
        {
            var winsCount = cards[i].WinsCount;
            for (var j = i + 1; j <= i + winsCount; j++)
            {
                numberOfCards[j] += numberOfCards[i];
            }
        }

        var result2 = numberOfCards.Sum();
        Console.WriteLine(result2);
    }

    private sealed record Card(List<int> WinningNumbers, List<int> YourNumbers)
    {
        public int WinsCount => this.YourNumbers.Count(x => this.WinningNumbers.Contains(x));
    }
}
