using System.Text.RegularExpressions;

namespace adventofcode2023;

public class Day2 : PuzzleBase
{
    private static readonly Regex LineRegex = new(@"^Game (\d+): (.*)$", RegexOptions.Compiled);
    private static readonly Regex GreenRegex = new(@"(\d+) green", RegexOptions.Compiled);
    private static readonly Regex RedRegex = new(@"(\d+) red", RegexOptions.Compiled);
    private static readonly Regex BlueRegex = new(@"(\d+) blue", RegexOptions.Compiled);

    private record Round(int Blue, int Red, int Green);

    private record Game(int Index, Round[] Rounds);

    public override void Solve()
    {
        var games = ParseGames(ReadLines());
        var result1 = games
            .Where(g => g.Rounds.All(r => r is { Red: <= 12, Green: <= 13, Blue: <= 14 }))
            .Sum(g => g.Index);

        Console.WriteLine(result1);

        var result2 = games.Select(game =>
        {
            var red = game.Rounds.Select(x => x.Red).Max();
            var blue = game.Rounds.Select(x => x.Blue).Max();
            var green = game.Rounds.Select(x => x.Green).Max();
            return red * blue * green;
        }).Sum();
        Console.WriteLine(result2);
    }

    private Game[] ParseGames(List<string> lines)
    {
        var games = new List<Game>();
        foreach (var rawGame in lines)
        {
            var match = LineRegex.Match(rawGame);
            var index = int.Parse(match.Groups[1].Value);
            var rawRounds = match.Groups[2].Value.Split(';');
            var rounds = new List<Round>();
            foreach (var rawRound in rawRounds)
            {
                var (blue, red, green) = (0, 0, 0);
                var rawRecords = rawRound.Split(",");
                foreach (var rawRecord in rawRecords)
                {
                    if (BlueRegex.IsMatch(rawRecord))
                        blue = int.Parse(BlueRegex.Match(rawRecord).Groups[1].Value);
                    else if (RedRegex.IsMatch(rawRecord))
                        red = int.Parse(RedRegex.Match(rawRecord).Groups[1].Value);
                    else if (GreenRegex.IsMatch(rawRecord))
                        green = int.Parse(GreenRegex.Match(rawRecord).Groups[1].Value);
                    else
                        throw new Exception($"Unexpected rawRecord {rawRecord}");
                }

                rounds.Add(new Round(blue, red, green));
            }

            games.Add(new Game(index, rounds.ToArray()));
        }

        return games.ToArray();
    }
}