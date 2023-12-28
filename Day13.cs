namespace adventofcode2023;

public class Day13 : PuzzleBase
{
    //public override string InputFileName => "sample.txt";

    public override void Solve()
    {
        var maps = SplitMaps(ReadLines());

        var result1 = 0;
        foreach (var map in maps)
        {
            result1 += Enumerable.Range(1, map.Count - 1).Select(x => IsHorizontalReflection(map, x) == 0 ? x * 100 : 0).Sum();
            result1 += Enumerable.Range(1, map[0].Count - 1).Select(x => IsVerticalReflection(map, x) == 0 ? x : 0).Sum();
        }
        Console.WriteLine(result1);

        var result2 = 0;
        foreach (var map in maps)
        {
            result2 += Enumerable.Range(1, map.Count - 1).Select(x => IsHorizontalReflection(map, x) == 1 ? x * 100 : 0).Sum();
            result2 += Enumerable.Range(1, map[0].Count - 1).Select(x => IsVerticalReflection(map, x) == 1 ? x : 0).Sum();
        }
        Console.WriteLine(result2);
    }

    private static List<List<List<char>>> SplitMaps(List<string> lines)
    {
        var maps = new List<List<List<char>>>();
        var map = new List<List<char>>();
        foreach (var line in lines)
        {
            if (line == "")
            {
                maps.Add(map);
                map = new List<List<char>>();
            }
            else
            {
                map.Add(line.ToList());
            }
        }

        maps.Add(map);
        return maps;
    }

    private static int IsHorizontalReflection(List<List<char>> map, int index)
    {
        var result = 0;
        var l = index - 1;
        var r = index;
        while (l >= 0 && r < map.Count)
        {
            for (var i = 0; i < map[0].Count; i++)
            {
                if (map[l][i] != map[r][i])
                {
                    result++;
                }
            }

            l--;
            r++;
        }

        return result;
    }

    private static int IsVerticalReflection(List<List<char>> map, int index)
    {
        var result = 0;
        var l = index - 1;
        var r = index;
        while (l >= 0 && r < map[0].Count)
        {
            for (var i = 0; i < map.Count; i++)
            {
                if (map[i][l] != map[i][r])
                {
                    result++;
                }
            }

            l--;
            r++;
        }

        return result;
    }
}
