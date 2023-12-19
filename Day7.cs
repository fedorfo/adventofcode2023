namespace adventofcode2023;

using System.Globalization;
using helpers;

public class Day7 : PuzzleBase
{
    private static readonly Dictionary<char, int> CardValue = new()
    {
        { 'A', 14 },
        { 'K', 13 },
        { 'Q', 12 },
        { 'J', 11 },
        { 'T', 10 },
        { '9', 9 },
        { '8', 8 },
        { '7', 7 },
        { '6', 6 },
        { '5', 5 },
        { '4', 4 },
        { '3', 3 },
        { '2', 2 }
    };

    public override void Solve()
    {
        var lines = ReadLines();
        var input = lines.Select(x =>
        {
            var tokens = Helpers.ExtractTokens(x).ToList();
            return new GameSet(tokens[0], int.Parse(tokens[1], CultureInfo.InvariantCulture));
        }).ToList();

        var sorted1 = input.OrderBy(x => GetCombination(x.Hand, false)).ThenBy(x => GetHandValue(x.Hand, false))
            .ToList();
        var result1 = sorted1.Select((x, i) => (i + 1) * x.Bid).Sum();
        Console.WriteLine(result1);

        var sorted2 = input.OrderBy(x => GetCombination(x.Hand, true)).ThenBy(x => GetHandValue(x.Hand, true)).ToList();
        var result2 = sorted2.Select((x, i) => (i + 1) * x.Bid).Sum();
        Console.WriteLine(result2);
    }

    private static Combination GetCombination(string hand, bool jMeansJoker)
    {
        var jokerCount = 0;
        if (jMeansJoker)
        {
            jokerCount = hand.Count(x => x == 'J');
            if (jokerCount == 5)
            {
                return Combination.Five;
            }

            hand = new string(hand.Where(x => x != 'J').ToArray());
        }

        var groups = hand.GroupBy(x => x).Select(x => (x.Key, Count: x.Count())).OrderByDescending(x => x.Count)
            .ToList();

        if (jokerCount > 0)
        {
            groups[0] = (groups[0].Key, groups[0].Count + jokerCount);
        }

        if (groups[0].Count == 5)
        {
            return Combination.Five;
        }

        if (groups[0].Count == 4)
        {
            return Combination.Four;
        }

        if (groups[0].Count == 3 && groups[1].Count == 2)
        {
            return Combination.FullHouse;
        }

        if (groups[0].Count == 3)
        {
            return Combination.Three;
        }

        if (groups[0].Count == 2 && groups[1].Count == 2)
        {
            return Combination.TwoPair;
        }

        if (groups[0].Count == 2)
        {
            return Combination.OnePair;
        }

        return Combination.HighCard;
    }

    private static long GetHandValue(string hand, bool jMeansJoker)
    {
        long result = 0;
        for (var i = 0; i < 5; i++)
        {
            result += (jMeansJoker && hand[i] == 'J' ? 1 : CardValue[hand[i]]) * Helpers.LongPow(15, 4 - i);
        }

        return result;
    }

    private enum Combination
    {
        Five = 6,
        Four = 5,
        FullHouse = 4,
        Three = 3,
        TwoPair = 2,
        OnePair = 1,
        HighCard = 0
    }

    private sealed record GameSet(string Hand, int Bid);
}
