namespace adventofcode2023;

using helpers;

public class Day14 : PuzzleBase
{
    public override void Solve()
    {
        var map = ReadLines().Select(x => x.ToList()).ToList();
        var result1 = CalculateLoad(MoveRoundRocks(map, new V2(-1, 0)));
        Console.WriteLine(result1);

        var prev = new Dictionary<string, int>();
        var it = 1000000000;
        var cycleLength = -1;
        while (true)
        {
            map = MoveRoundRocks(map, new V2(-1, 0));
            map = MoveRoundRocks(map, new V2(0, -1));
            map = MoveRoundRocks(map, new V2(1, 0));
            map = MoveRoundRocks(map, new V2(0, 1));
            it--;
            var cur = new string(map.SelectMany(x => x).ToArray());
            if (!prev.ContainsKey(cur))
            {
                prev[cur] = it;
            }
            else
            {
                cycleLength = prev[cur] - it;
                break;
            }
        }

        it %= cycleLength;
        while (it > 0)
        {
            map = MoveRoundRocks(map, new V2(-1, 0));
            map = MoveRoundRocks(map, new V2(0, -1));
            map = MoveRoundRocks(map, new V2(1, 0));
            map = MoveRoundRocks(map, new V2(0, 1));
            it--;
        }

        Console.WriteLine(cycleLength);
        var result2 = CalculateLoad(map);
        Console.WriteLine(result2);
    }

    private static int CalculateLoad(List<List<char>> map)
    {
        var result = 0;
        for (var i = 0; i < map.Count; i++)
        {
            for (var j = 0; j < map[0].Count; j++)
            {
                if (map[i][j] == 'O')
                {
                    result += map.Count - i;
                }
            }
        }

        return result;
    }

    private static List<List<char>> MoveRoundRocks(List<List<char>> map, V2 direction)
    {
        var result = map.Select(x => x.ToList()).ToList();
        for (var k = 0; k < Math.Max(result.Count, result[0].Count); k++)
        {
            for (var i = 0; i < result.Count; i++)
            {
                for (var j = 0; j < result[0].Count; j++)
                {
                    var v = new V2(i, j);
                    var u = v + direction;
                    if (!(u >= V2.Zero && u < new V2(map.Count, map[0].Count)))
                    {
                        continue;
                    }

                    if (result[v.X][v.Y] == 'O' && result[u.X][u.Y] == '.')
                    {
                        result[u.X][u.Y] = 'O';
                        result[v.X][v.Y] = '.';
                    }
                }
            }
        }

        return result;
    }
}
