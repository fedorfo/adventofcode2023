namespace adventofcode2023;

using helpers;

public class Day11 : PuzzleBase
{
    public override void Solve()
    {
        var map = ReadLines().Select(x => x.ToList()).ToList();
        var galaxies = new List<V2>();
        for (var i = 0; i < map.Count; i++)
        {
            for (var j = 0; j < map[0].Count; j++)
            {
                if (map[i][j] == '#')
                {
                    galaxies.Add(new V2(i, j));
                }
            }
        }

        var emptyRows = map.Select((_, i) => Enumerable.Range(0, map[0].Count).All(j => map[i][j] == '.') ? i : -1)
            .Where(x => x != -1)
            .ToList();
        var emptyCols = map[0].Select((_, j) => Enumerable.Range(0, map.Count).All(i => map[i][j] == '.') ? j : -1)
            .Where(x => x != -1).ToList();

        var result1 = GetResult(galaxies, emptyRows, emptyCols, 2);
        Console.WriteLine(result1);

        var result2 = GetResult(galaxies, emptyRows, emptyCols, 1000000);
        Console.WriteLine(result2);
    }

    private static long GetResult(List<V2> galaxies, List<int> emptyRows, List<int> emptyCols, long emptyLineDelta)
    {
        long result = 0;
        for (var i = 0; i < galaxies.Count; i++)
        {
            for (var j = i + 1; j < galaxies.Count; j++)
            {
                for (var k = Math.Min(galaxies[i].X, galaxies[j].X); k < Math.Max(galaxies[i].X, galaxies[j].X); k++)
                {
                    result += emptyRows.Contains((int)k) ? emptyLineDelta : 1;
                }

                for (var k = Math.Min(galaxies[i].Y, galaxies[j].Y); k < Math.Max(galaxies[i].Y, galaxies[j].Y); k++)
                {
                    result += emptyCols.Contains((int)k) ? emptyLineDelta : 1;
                }
            }
        }

        return result;
    }
}
