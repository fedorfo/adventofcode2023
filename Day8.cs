namespace adventofcode2023;

using helpers;

public class Day8 : PuzzleBase
{
    private static readonly char[] Delimiters = { ' ', '=', ',', '(', ')' };

    public override void Solve()
    {
        var lines = ReadLines();
        var path = lines[0];
        var v = new Dictionary<string, Tuple<string, string>>();
        for (var i = 2; i < lines.Count; i++)
        {
            var tokens = Helpers.ExtractTokens(lines[i], Delimiters).ToList();
            v[tokens[0]] = Tuple.Create(tokens[1], tokens[2]);
        }

        var result1 = Steps(v, path, "AAA", x => x == "ZZZ");
        Console.WriteLine(result1);


        long result2 = 0;
        var curs = v.Keys.Where(x => x.EndsWith("A", StringComparison.InvariantCulture)).ToList();
        var res = curs.Select(x =>
            Steps(v, path, x, y => y.EndsWith("Z", StringComparison.InvariantCulture))
        ).ToList();
        result2 = 1;
        foreach (var x in res)
        {
            result2 = Helpers.Lcd(result2, x);
        }

        Console.WriteLine(result2);
    }

    private static int Steps(Dictionary<string, Tuple<string, string>> v, string path, string start,
        Func<string, bool> isEnd)
    {
        var result = 0;
        var idx = 0;
        while (!isEnd(start))
        {
            result++;
            if (!v.ContainsKey(start))
            {
                return result;
            }

            start = path[idx] == 'L' ? v[start].Item1 : v[start].Item2;
            idx++;
            if (idx == path.Length)
            {
                idx = 0;
            }
        }

        return result;
    }
}
